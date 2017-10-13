using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Segment : IEquatable<Segment>
    {
        public Segment(Point a, Point b)
        {
            Item1 = a;
            Item2 = b;
        }
        public Point Item1 { get; set; }
        public Point Item2 { get; set; }
        public Point Middle => new Point((Item1.X + Item2.X) / 2, (Item1.Y + Item2.Y) / 2);

        public int Length => (Item1.X - Item2.X) * (Item1.X - Item2.X) + (Item1.Y - Item2.Y) * (Item1.Y - Item2.Y);

        public int DistanceTo(Point c)
        {
            return Math.Abs((Item1.X - c.X) * (Item2.Y - c.Y) - (Item2.X - c.X) * (Item1.Y - c.Y))
                   / (Length > 0 ? Length : 1);
        }

        public bool IsVertical => Item1.X == Item2.X;

        public bool IsHorizontal => Item1.Y == Item2.Y;

        public void OrderFromLeftToRight()
        {
            if (Item2.X < Item1.X)
            {
                var s = Item1;
                Item1 = Item2;
                Item2 = s;
            }
        }

        public bool Equals(Segment other)
        {
            if (other == null)
            {
                return false;
            }
            return Item1.Equals(other.Item1) && Item2.Equals(other.Item2)
                || Item1.Equals(other.Item2) && Item2.Equals(other.Item1);
        }

        public override bool Equals(object other)
        {
            var a = other as Segment;
            return a == null || Equals(a);
        }
    }
}