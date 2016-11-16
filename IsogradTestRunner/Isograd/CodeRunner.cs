using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Colorful;
using IsogradTestRunner.Extensions;
using IsogradTestRunner.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using Console = Colorful.Console;

namespace IsogradTestRunner.Isograd
{
    public class CodeRunner
    {
        private readonly string _sourceCodeFile;
        private readonly IEnumerable<string> _inputFiles;
        private readonly CodeRunnerParameters _parameters;
        private readonly StyleSheet _styleSheet;

        public CodeRunner(string sourceCodeFile, IEnumerable<string> inputFiles, CodeRunnerParameters parameters)
        {
            _sourceCodeFile = sourceCodeFile;
            _inputFiles = inputFiles;
            _parameters = parameters;

            _styleSheet = new StyleSheet(Color.White);
            _styleSheet.AddStyle("OK", Color.Green);
            _styleSheet.AddStyle("KO", Color.Red);
        }

        public void CompileAndRunTests()
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_sourceCodeFile);

            var compilation =
                CSharpCompilation.Create
                    (
                        assemblyName: fileNameWithoutExtension,
                        syntaxTrees: new[] { CSharpSyntaxTree.ParseText(FileWithoutLock.ReadAllText(_sourceCodeFile)) },
                        references: new []
                        {
                            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),      //mscorlib.dll
                            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),  //System.Core.dll
                            MetadataReference.CreateFromFile(typeof(Uri).Assembly.Location)          //System.dll
                        },
                        options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                    );

            var buildedMethod = CompilationHelper.For(compilation)
                .TryEmit(
                    onSuccess: assembly =>
                        assembly
                            .GetTypes()
                            .Single(t => t.Name == fileNameWithoutExtension)
                            .GetMethod(_parameters.StaticMethodToRun),
                    onErrorAction: diagnostic =>
                    {
                        if (diagnostic.Severity == DiagnosticSeverity.Error || diagnostic.IsWarningAsError)
                        {
                            var color = diagnostic.IsWarningAsError ? Color.Orange : Color.Red;
                            Console.WriteLine($"{diagnostic.Location.ToString()}:{diagnostic.Id}: {diagnostic.GetMessage()}", color);
                        }
                    });
            RunTests(buildedMethod);
        }

        private void RunTests(MethodInfo methodInfo)
        {
            if (methodInfo == null) return;

            if (!_inputFiles.Any())
            {
                Console.WriteLine($"WARN: no input files found in {Path.GetDirectoryName(_sourceCodeFile)}", Color.Yellow);
            }

            foreach (var inputFile in _inputFiles)
            {
                var outputFile = Path.Combine(
                    Path.GetDirectoryName(inputFile),
                    Path.GetFileName(inputFile.Replace("input", "output")));

                if (File.Exists(outputFile))
                {
                    RunSingleTest(methodInfo, inputFile, outputFile);
                }
                else
                {
                    Console.WriteLine($"WARN: {inputFile} ignore because no associated output was found", Color.Yellow);
                }
            }
        }

        private void RunSingleTest(MethodInfo methodInfo, string inputFile, string outputFile)
        {
            Console.SetIn(File.OpenText(inputFile));
            var actualOutput = new StringWriter();
            var expectedOutputLines = FileWithoutLock.ReadAllLines(outputFile).Select(l => l.Replace("\r\n", string.Empty)).ToArray();

            Console.SetOut(actualOutput);
            methodInfo.Invoke(null, null);
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true});

            var actualOutputLines= new StringReader(actualOutput.ToString()).ReadAllLines()
                .Select(l => l.Replace("\r\n", string.Empty)).ToArray();
            var success = CompareOutput(actualOutputLines, expectedOutputLines);
            var sucessLabel = success ? "OK" : "KO";

            Console.WriteLineStyled($"[{Path.GetFileName(_sourceCodeFile)}] Running test : {Path.GetFileName(inputFile)} {sucessLabel}", _styleSheet);

            if (!success)
            {
                PrintDiff(expectedOutputLines, actualOutputLines);
            }
        }

        private bool CompareOutput(IReadOnlyCollection<string> actualOutput, IReadOnlyList<string> expectedOutput)
        {
            return actualOutput.Count == expectedOutput.Count && !actualOutput.Where((str, i) => !str.Equals(expectedOutput[i])).Any();
        }

        private void PrintDiff(IReadOnlyList<string> expectedOutputLines, IReadOnlyList<string> actualOutputLines)
        {
            var maxLength = Math.Max(expectedOutputLines.Max(l => l.Length), "Expecting: ".Length);

            Console.Write("Expected: ".PadRight(maxLength), Color.DimGray);
            Console.WriteLine("Actual:", Color.White);
            for (int i = 0; i < Math.Max(expectedOutputLines.Count, actualOutputLines.Count); i++)
            {
                var expectedLine = i < expectedOutputLines.Count ? expectedOutputLines[i] : string.Empty;
                var actualLine   = i < actualOutputLines.Count   ? actualOutputLines[i]   : string.Empty;

                Console.Write(expectedLine.PadRight(maxLength), Color.DimGray);
                Console.WriteLine(actualLine, Color.White);
            }
        }

    }
}