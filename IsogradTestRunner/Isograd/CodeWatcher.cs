using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using IsogradTestRunner.Extensions;
using IsogradTestRunner.Helpers;
using Console = Colorful.Console; 

namespace IsogradTestRunner.Isograd
{
    public class CodeWatcher : IDisposable
    {
        private readonly CodeRunnerParameters _parameters;
        private readonly FileSystemWatcher _fsw;
        private readonly BlockingCollection<string> _workQueue;
        private readonly PoorManDebouncer<FileSystemEventArgs, string> _debouncer;

        public CodeWatcher(CodeRunnerParameters parameters)
        {
            _parameters = parameters;

            _workQueue = new BlockingCollection<string>();
            _debouncer = new PoorManDebouncer<FileSystemEventArgs, string>(
                actionToDebounce: evt =>
                {
                    Console.WriteLine($"INFO: {Path.GetFileName(evt.FullPath)} changed detected !", Color.SaddleBrown);
                    _workQueue.Add(evt.FullPath);
                },
                debounceBy: evt => evt.FullPath,
                delay: 100.Milliseconds());

            _fsw = new FileSystemWatcher
            {
                Path = _parameters.Directory,
                IncludeSubdirectories = true,
                Filter = "*.cs",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Security,
                EnableRaisingEvents = true,
            };
           
            _fsw.Changed += OnSourceFileChanged;

            Console.WriteLine($"INFO: Running and watching for change in {_parameters.Directory}", Color.SaddleBrown);

            StartConsumingTask();
        }

        private void OnSourceFileChanged(object _, FileSystemEventArgs fileSystemEventArgs)
        {
            _debouncer.DebouncedActionFor(fileSystemEventArgs);
        }

        private void StartConsumingTask()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var sourceCodeFile = _workQueue.Take();
                    var directory = Path.GetDirectoryName(sourceCodeFile);

                    var inputFiles = Directory
                        .GetFiles(directory, _parameters.InputFilePattern, SearchOption.TopDirectoryOnly);

                    new CodeRunner(sourceCodeFile, inputFiles, _parameters).CompileAndRunTests();
                }
            });
        }

        public void Dispose()
        {
            _fsw.Dispose();
        }
    }
}