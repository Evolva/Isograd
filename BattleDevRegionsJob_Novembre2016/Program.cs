using System;
using System.IO;

namespace BattleDevRegionsJob_Novembre2016
{
    public class Program
    {
        public static void Main()
        {
            Console.SetIn(File.OpenText(@"..\..\5.Snowboarding\input2.txt"));
            BattleDevRegionsJob_Novembre2016._5.Snowboarding.Snowboarding.Solve();
        }
    }
}