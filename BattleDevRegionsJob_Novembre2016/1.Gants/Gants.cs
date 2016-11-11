using System;
using System.Linq;

namespace BattleDevRegionsJob_Novembre2016._1.Gants
{
    public class Gants
    {
        public static void Solve()
        {
            var input = Console.In;
            var nbLine = int.Parse(input.ReadLine());

            var gloves = Enumerable
                .Range(0, nbLine)
                .Select(_ => input.ReadLine());

            var pair = gloves
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count())
                .Sum(kvp => kvp.Value / 2);

            Console.WriteLine(pair);
        }
    }
}