using System;

namespace BattleDevRegionsJob_Novembre2016._2.Flocons
{
    public static class Flocons
    {
        public static void Solve()
        {
            var input = Console.In;
            var size = int.Parse(input.ReadLine());

            for (var nbStar = 1; nbStar < size; nbStar += 2)
            {
                var nbDot = (size - nbStar)/2;
                Console.Write(new string('.', nbDot));
                Console.Write(new string('*', nbStar));
                Console.WriteLine(new string('.', nbDot));
            }

            for (var nbStar = size; nbStar >= 1; nbStar -= 2)
            {
                var nbDot = (size - nbStar)/2;
                Console.Write(new string('.', nbDot));
                Console.Write(new string('*', nbStar));
                Console.WriteLine(new string('.', nbDot));
            }
        }
    }
}
