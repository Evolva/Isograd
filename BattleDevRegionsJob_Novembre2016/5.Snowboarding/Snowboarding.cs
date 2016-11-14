﻿using System;
using System.Linq;

namespace BattleDevRegionsJob_Novembre2016._5.Snowboarding
{
    public class Snowboarding
    {
        public static void Solve()
        {
            var input = Console.In;
            var size = int.Parse(input.ReadLine());
 
            var circles = Enumerable
                .Range(0, size)
                .Select(_ => input.ReadLine())
                .Select(x =>
                {
                    var c = x.Split().Select(int.Parse).ToArray();
                    return new Circle(c[0], c[1], c[2], c[3]);
                }).ToList();
            circles.Add(new Circle(100000 / 2, 100000 / 2, 100000 * 2, 0));

        }

        public class Circle
        {
            public Circle(int x, int y, int r, int h)
            {
                X = x;
                Y = y;
                R = r;
                H = h;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int R { get; set; }
            public int H { get; set; }

            public double Dist(Circle other)
            {
                return Math.Sqrt
                    (
                        Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2)
                    );
            }

            public bool IsInside(Circle other)
            {
                var minR = Math.Min(R, other.R);
                var maxR = Math.Max(R, other.R);
                var dist = this.Dist(other);

                return dist <= minR + maxR && dist + minR < maxR;
            }

            public int Deniv(Circle other)
            {
                return Math.Abs(H - other.H);
            }
        }
    }
}