using System;

namespace GeometryTasks
{
    public class Vector
    {
        public double X;
        public double Y;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public Vector Add(Vector v2)
        {
            return Geometry.Add(this, v2);
        }

        public bool Belongs(Segment s)
        {
            return Geometry.IsVectorInSegment(this, s);
        }
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public bool Contains(Vector v)
        {
            return Geometry.IsVectorInSegment(v, this);
        }
    }

    public class Geometry
    {
        public static double GetLength(Vector v)
        {
            //return Math.Sqrt(v.X * v.X + v.Y * v.Y);
            return GetLength(v.X, v.Y, 0, 0);
        }

        public static Vector Add(Vector v1, Vector v2)
        {
            return new Vector { X = v1.X + v2.X, Y = v1.Y + v2.Y };
        }

        public static double GetLength(Segment s)
        {
            return GetLength(s.Begin.X, s.Begin.Y, s.End.X, s.End.Y);
        }

        public static double GetLength(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public static bool IsVectorInSegment(Vector v, Segment s)
        {
            // Segment ab = s;
            var ax = GetLength(s.Begin.X, s.Begin.Y, v.X, v.Y);
            var xb = GetLength(v.X, v.Y, s.End.X, s.End.Y);

            return ax + xb == GetLength(s); // ax + xb = ab
        }
    }
}