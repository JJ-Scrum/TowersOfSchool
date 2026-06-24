namespace TowersOfSchool.Models
{
    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator *(Point a, double scalar)
        {
            return new Point(a.X * scalar, a.Y * scalar);
        }

        public readonly double DistanceTo(double x, double y)
        {
            double dx = x - X;
            double dy = y - Y;
            return System.Math.Sqrt(dx * dx + dy * dy);
        }

        public readonly double DistanceTo(Point other)
        {
            return DistanceTo(other.X, other.Y);
        }
    }
}
