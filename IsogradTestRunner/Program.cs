using System;
using System.IO;
using IsogradTestRunner.Isograd;

namespace IsogradTestRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var codeRunnerParam = new CodeRunnerParameters();
            if (CommandLine.Parser.Default.ParseArguments(args, codeRunnerParam))
            {
                if (!Directory.Exists(codeRunnerParam.Directory))
                {
                    Console.WriteLine(codeRunnerParam.Directory + " don't exist !");
                    Console.WriteLine();
                    Console.WriteLine(codeRunnerParam.GetUsage());
                    Console.ReadKey();
                    return;
                }

                using (new CodeWatcher(codeRunnerParam))
                {
                    Console.ReadLine();
                }
            }
        }
    }
}