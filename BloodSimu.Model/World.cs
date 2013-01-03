using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BloodSimu.Model
{
    public class World
    {
        public Collection<Particle> Particles;

        public Collection<Border> Borders;

        public Collection<AccelerationArea> AccelerationAreas;

        public World(Collection<Border> borders, Collection<Particle> particles, Collection<AccelerationArea> areas)
        {
            Borders = borders;
            Particles = particles;
            AccelerationAreas = areas;
        }

        public void Move(TimeSpan deltaTime)
        {
            Parallel.ForEach(Particles, particle =>
                {
                    // TODO: Collistion detection with border

                    var initialPosition = particle.Position;

                    foreach (var area in AccelerationAreas.Where(a => a.IsInRange(particle)))
                    {
                        particle.SetAcceleration(area.GetAcceleration(particle));
                    }

                    particle.Move(deltaTime);

                    var newPosition = particle.Position;

                    foreach (var border in Borders)
                    {
                        var side1 = border.GetSide(initialPosition);
                        var side2 = border.GetSide(newPosition);
                        if (side1 != side2 && border.IsVectorCrossingBorder(initialPosition, newPosition))
                        {
                            particle.UndoLastMove();
                            particle.Bump(border.CollisionVector);
                        }
                    }
                });
        }

        public void RemoveParticle(Particle particle)
        {
            Particles.Remove(particle);
        }

        public void AddParticle(Particle particle)
        {
            Particles.Add(particle);
        }
    }

    public class AccelerationArea
    {
        public AccelerationArea(Vector2D start, Vector2D end, int range, Vector2D maxForce)
        {
            Start = start;
            End = end;
            Range = range;
            Force = maxForce;
        }

        public Vector2D Start { get; set; }
        public Vector2D End { get; set; }

        public Vector2D Force { get; set; }

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

            //var A = Start;
            //var B = End;
            //var P = particle.Position;

            //double normalLength = (B - A).GetLength();
            //var distance = Math.Abs(((P.X - A.X)*(B.Y - A.Y) - (P.Y - A.Y)*(B.X - A.X))/normalLength);
            //return distance;
        }

        protected double Range { get; private set; }

        public Vector2D GetAcceleration(Particle particle)
        {
            var distance = GetDistance(particle);
            if (distance < 1)
                distance = 1;

            return Force/distance;
        }
    }
}