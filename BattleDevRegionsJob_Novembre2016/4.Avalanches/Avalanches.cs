using System;
using System.Globalization;
using System.Linq;

namespace BattleDevRegionsJob_Novembre2016._4.Avalanches
{
    public class Avalanches
    {
        public static void Solve()
        {
            var input = Console.In;
            var size = int.Parse(input.ReadLine());

            var fromTo = input.ReadLine().Split().Select(int.Parse).ToArray();

            var from = fromTo[0];
            var to = fromTo[1];

            var matrix = Enumerable
                .Range(0, size)
                .Select(_ => input.ReadLine())
                .Select(x => x.Split().Select(y => 1 - double.Parse(y, CultureInfo.InvariantCulture)).ToArray())
                .ToArray();

            for (var k = 0; k < size; k++)
            {
                for (var i = 0; i < size; i++)
                {
                    for (var j = 0; j < size; j++)
                    {
                        matrix[i][j] = Math.Max(matrix[i][j], matrix[i][k] * matrix[k][j]);
                    }
                }
            }
            var probability = (1 - matrix[from][to]).ToString("#0.000", CultureInfo.InvariantCulture);
            Console.WriteLine(probability);
        }
    }
}