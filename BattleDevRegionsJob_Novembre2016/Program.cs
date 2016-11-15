using System;
using System.IO;
using BattleDevRegionsJob_Novembre2016._5.Snowboarding;

namespace BattleDevRegionsJob_Novembre2016
{
    public class Program
    {
        public static void Main()
        {
            Console.SetIn(File.OpenText(@"..\..\5.Snowboarding\input2.txt"));
            Snowboarding.Solve();
        }
    }
}