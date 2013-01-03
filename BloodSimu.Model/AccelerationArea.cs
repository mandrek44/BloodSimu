using System;

namespace BloodSimu.Model
{
    public class AccelerationArea
    {
        public AccelerationArea(Vector2D start, Vector2D end, int range, Vector2D maxForce)
        {
            Start = start;
            End = end;
            Range = range;
            Force = maxForce;
        }

        public Vector2D Start { get; private set; }

        public Vector2D End { get; private set; }

        protected double Range { get; private set; }

        public Vector2D Force { get; private set; }

        public bool IsInRange(Particle particle)
        {
            var distance = GetDistance(particle);

            return distance < Range;
        }

        private double GetDistance(Particle particle)
        {
            var p = particle.Position;

            var squaredLength = (End - Start).GetSquaredLength();

            var lengthToA = (p - Start).GetSquaredLength();
            var lengthToB = (p - End).GetSquaredLength();
            if (lengthToA > squaredLength || lengthToB > squaredLength)
                return Math.Min(lengthToA, lengthToB);

            var a = Start;
            var n = (End - Start).Normalize();
            var distance = ((a - p) - n * ((a - p).Dot(n))).GetLength();

            return distance;
        }

        public Vector2D GetAcceleration(Particle particle)
        {
            var distance = GetDistance(particle);
            if (distance < 1)
                distance = 1;

            return Force/distance;
        }
    }
}