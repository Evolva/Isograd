using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BattleDevRegionsJob_Novembre2016._5.Snowboarding
{
    public class Snowboarding
    {
        public static void Solve() { }
    }

    #region seems to be working, but to slow for isograd ;'(
    public class Slowboarding
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
            var backgroundCircle = new Circle(100000 / 2, 100000 / 2, 100000 * 2, 0);
            circles.Add(backgroundCircle);

            var availableNeighborsFrom = circles.ToDictionary(
                keySelector: c => c,
                elementSelector: c =>
                {
                    var insideCurrentCircle = circles.Where(x => x.IsInsideOf(c)).ToList();
                    var butNotInsideAnotherSmallerCircle = insideCurrentCircle.Where(x => !insideCurrentCircle.Any(y => x.IsInsideOf(y))).ToList();

                    var firstBiggerCircle = circles.Where(x => x.IsIncludeIn(c)).OrderBy(x => x.R).FirstOrDefault();
                    if (firstBiggerCircle != null)
                    {
                        butNotInsideAnotherSmallerCircle.Add(firstBiggerCircle);
                    }
                    return butNotInsideAnotherSmallerCircle;
                });

            var decisionTree = new Tree<State>(new State(0, Enumerable.Empty<Circle>(), null));

            foreach (var circle in circles)
            {
                decisionTree.AddChildren(new State(0, new[] { circle }, circle));
            }

            var stack = new Stack<Tree<State>>(decisionTree.Children);

            while (stack.Any())
            {
                var elt = stack.Pop();
                var currentCircle = elt.NodeState.CurrentCircle;

                var possibleChildren = availableNeighborsFrom[currentCircle].Except(elt.NodeState.AlreadyVisitedCircles);

                foreach (var child in possibleChildren)
                {
                    var heightDifference = elt.NodeState.SumHeighDifference + currentCircle.HeightDifference(child);
                    var newChild = elt.AddChildren(new State(heightDifference, elt.NodeState.AlreadyVisitedCircles.Union(new[] { child }),
                        child));

                    stack.Push(newChild);
                }
            }

            var maxHeightDiffenrence = decisionTree.FindMax(x => x.SumHeighDifference);
            Console.WriteLine(maxHeightDiffenrence);
        }

        [DebuggerDisplay("X:{X}, Y:{Y}, R:{R}, H:{H}")]
        public class Circle : IEquatable<Circle>
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

            private double Dist(Circle other)
            {
                return Math.Sqrt
                    (
                        Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2)
                    );
            }

            public bool IsInsideOf(Circle other)
            {
                var maxR = Math.Max(R, other.R);

                if (maxR != other.R) return false;

                var minR = Math.Min(R, other.R);
                var dist = Dist(other);

                return dist <= minR + maxR && dist + minR < maxR;
            }

            public bool IsIncludeIn(Circle other)
            {
                var maxR = Math.Max(R, other.R);

                if (maxR == other.R) return false;

                var minR = Math.Min(R, other.R);
                var dist = Dist(other);

                return dist <= minR + maxR && dist + minR < maxR;
            }

            public int HeightDifference(Circle other)
            {
                return Math.Abs(H - other.H);
            }

            public bool Equals(Circle other)
            {
                if (ReferenceEquals(this, other)) return true;
                if (other == null) return false;

                return X == other.X && Y == other.Y && R == other.R && H == other.H;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = X;
                    hashCode = (hashCode * 397) ^ Y;
                    hashCode = (hashCode * 397) ^ R;
                    hashCode = (hashCode * 397) ^ H;
                    return hashCode;
                }
            }
        }

        public class State
        {
            public State(int sumHeighDifference, IEnumerable<Circle> alreadyVisitedCircles, Circle currentCircle)
            {
                SumHeighDifference = sumHeighDifference;
                AlreadyVisitedCircles = alreadyVisitedCircles;
                CurrentCircle = currentCircle;
            }

            public int SumHeighDifference { get; private set; }
            public Circle CurrentCircle { get; private set; }
            public IEnumerable<Circle> AlreadyVisitedCircles { get; private set; }
        }

        public class Tree<T>
        {
            public T NodeState { get; private set; }
            public IList<Tree<T>> Children { get; private set; }

            public Tree(T nodeState)
            {
                NodeState = nodeState;
                Children = new List<Tree<T>>();
            }

            public Tree<T> AddChildren(T childrenState)
            {
                var newChildren = new Tree<T>(childrenState);
                Children.Add(newChildren);
                return newChildren;
            }

            public int FindMax(Func<T, int> valueSelector)
            {
                return Math.Max(valueSelector(NodeState), Children.Any() ? Children.Max(c => c.FindMax(valueSelector)) : int.MinValue);
            }
        }
    }
    #endregion
}