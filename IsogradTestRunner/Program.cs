using System;
using System.IO;
using IsogradTestRunner.Isograd;

namespace IsogradTestRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string directory = @"D:\Workspace\CSharp\Isograd\BattleDevRegionsJob_Novembre2016";

            if (!Directory.Exists(directory))
            {
                Console.WriteLine(directory + " don't exist !");
                return;
            }

            var isogradParameters = new CodeRunnerParameters
            {
                Directory = directory
            };

            using (new CodeWatcher(isogradParameters))
            {
                Console.ReadLine();
            }            
        }
    }
}