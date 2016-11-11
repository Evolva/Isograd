using System;
using System.Linq;

namespace BattleDevRegionsJob_Novembre2016._3.Topographie
{
    public class Topographie
    {
        public static void Solve()
        {
            var input = Console.In;
            var nbCircles = int.Parse(input.ReadLine());

            var circles = Enumerable
                .Range(0, nbCircles)
                .Select(_ => input.ReadLine())
                .Select(x =>
                {
                    var c = x.Split(' ').Select(int.Parse).ToArray();
                    return new Circle(c[0], c[1], c[2]);
                }).ToArray();

            for (var i = 0; i < circles.Length; i++)
            {
                for (var j = i + 1; j < circles.Length; j++)
                {
                    var distance = circles[i].Dist(circles[j]);
                    var minR = Math.Min(circles[i].R, circles[j].R);
                    var maxR = Math.Max(circles[i].R, circles[j].R);

                    if (distance == 0 && minR != maxR) continue;
                    if (distance > minR + maxR) continue;
                    if (distance < minR + maxR && distance + minR < maxR) continue;

                    Console.WriteLine("KO");
                    return;
                }
            }

            Console.WriteLine("OK");
        }

        public class Circle
        {
            public Circle(int x, int y, int r)
            {
                X = x;
                Y = y;
                R = r;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int R { get; set; }

            public double Dist(Circle other)
            {
                return Math.Sqrt
                    (
                        Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2)
                    );
            }
        }
    }
}