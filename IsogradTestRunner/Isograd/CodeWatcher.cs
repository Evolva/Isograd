using System;
using System.Drawing;
using System.IO;
using Console = Colorful.Console; 

namespace IsogradTestRunner.Isograd
{
    public class CodeWatcher : IDisposable
    {
        private readonly CodeRunnerParameters _parameters;
        private readonly FileSystemWatcher _fsw;

        public CodeWatcher(CodeRunnerParameters parameters)
        {
            _parameters = parameters;

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
        }

        private void OnSourceFileChanged(object _, FileSystemEventArgs fileSystemEventArgs)
        {
            var sourceCodeFile = fileSystemEventArgs.FullPath;
            Console.WriteLine($"INFO: {Path.GetFileName(sourceCodeFile)} changed detected !", Color.SaddleBrown);

            var directory = Path.GetDirectoryName(sourceCodeFile);

            var inputFiles = Directory
                .GetFiles(directory, _parameters.InputFilePattern, SearchOption.TopDirectoryOnly);

            new CodeRunner(sourceCodeFile, inputFiles, _parameters).CompileAndRunTests();
        }

        public void Dispose()
        {
            _fsw.Dispose();
        }
    }
}