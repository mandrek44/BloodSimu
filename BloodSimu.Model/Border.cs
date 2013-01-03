using System;

namespace BloodSimu.Model
{
    public class Border : WorldElement
    {
        public Border(Vector2D start, Vector2D end)
        {
            Start = start;
            End = end;
            CollisionVector = (End - Start).Perpendicular();
        }

        public Vector2D Start { get; private set; }

        public Vector2D End { get; private set; }

        public Vector2D CollisionVector { get; private set; }

        public int GetSide(Vector2D point)
        {
            var difference = End - Start;

            if (Math.Abs(difference.X) < 0.000001)
                return point.X < Start.X ? 1 : -1;

            var a = difference.Y/difference.X;
            var b = Start.Y - a*Start.X;

            var bprim = point.Y - a*point.X;

            return b > bprim ? 1 : -1;
        }


        public Vector2D GetIntersectionPoint(Vector2D start, Vector2D end)
        {
            var vector1 = End - Start;
            var vector2 = end - start;

            var a = vector1.GetSlope();
            var b = vector1.GetIntercept(Start);

            var c = vector2.GetSlope();
            var d = vector2.GetIntercept(start);

            if (Math.Abs(vector2.X) < 0.000001 && Math.Abs(vector1.X) < 0.000001)
                return start;

            if (Math.Abs(vector2.X) < 0.000001)
            {
                return new Vector2D(start.X, start.X *a + b);
            }
            
            if (Math.Abs(vector1.X) < 0.000001)
            {
                return new Vector2D(Start.X, Start.X*c + d);
            }

            var y = (-a*d + b)/(c - a);
            var x = (y - d)/c;

            return new Vector2D(x, y);
        }

        public bool IsVectorCrossingBorder(Vector2D start, Vector2D end)
        {
            var intersectionPoint = GetIntersectionPoint(start, end);

            if (Math.Abs(Start.X - End.X) < 0.00000001)
                return intersectionPoint.Y >= Start.Y && intersectionPoint.Y <= End.Y;
            return intersectionPoint.X >= Start.X && intersectionPoint.X <= End.X;
        }
    }

    public class WorldElement
    {
    }
}