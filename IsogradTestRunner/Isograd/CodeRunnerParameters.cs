namespace IsogradTestRunner.Isograd
{
    public class CodeRunnerParameters
    {
        public string Directory { get; set; }

        public string StaticMethodToRun { get; set; } = "Solve";

        public string InputFilePattern  { get; set; } = "input*.txt";
        //public string OutputFilePattern { get; set; } = "output*.txt";
    }
}