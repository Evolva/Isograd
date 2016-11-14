using System.Reflection;
using CommandLine;
using CommandLine.Text;

namespace IsogradTestRunner.Isograd
{
    public class CodeRunnerParameters
    {
        [ValueOption(0)]
        public string Directory { get; set; }

        [Option(longName: "method", shortName: 'm', DefaultValue = "Solve", HelpText = "Name of the method to run")]
        public string StaticMethodToRun { get; set; }

        public string InputFilePattern  { get; private set; } = "input*.txt";
        
        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("IsogradTestRunner", GetVersion()),
                Copyright = new CopyrightInfo("Evolva", 2017),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine("Usage: IsogradTestRunner <directory_to_watch> [-m <method_name>]");
            help.AddOptions(this);
            return help;
        }

        private static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}