using System;

namespace BloodSimu.Model
{
    public class Vector2D
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public readonly static Vector2D Zero = new Vector2D(0, 0);

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector2D operator +(Vector2D left, Vector2D right)
        {
            return new Vector2D(left.X + right.X, left.Y + right.Y);
        }

        public static Vector2D operator -(Vector2D left, Vector2D right)
        {
            return new Vector2D(left.X - right.X, left.Y - right.Y);
        }

        public static Vector2D operator *(Vector2D left, double c)
        {
            return new Vector2D(left.X * c, left.Y * c);
        }

        public static Vector2D operator /(Vector2D left, double c)
        {
            return new Vector2D(left.X / c, left.Y / c);
        }

        protected bool Equals(Vector2D other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector2D)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", X, Y);
        }

        public double GetSquaredLength()
        {
            return X * X + Y * Y;
        }

        public double GetLength()
        {
            return Math.Sqrt(GetSquaredLength());
        }

        public Vector2D Normalize()
        {
            var length = GetLength();
            return new Vector2D(X / length, Y / length);
        }

        public Vector2D Perpendicular()
        {
            return new Vector2D(-Y, X);
        }

        public double Dot(Vector2D other)
        {
            return X * other.X + Y * other.Y;
        }
    }
}