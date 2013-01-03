using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BloodSimu.Model
{
    public class World
    {
        public Collection<StopArea> StopAreas;
        public Collection<Particle> Particles;
        public Collection<Border> Borders;
        public Collection<AccelerationArea> AccelerationAreas;

        public World(Collection<Border> borders, Collection<Particle> particles, Collection<AccelerationArea> accelerationAreas, Collection<StopArea> stopAreas)
        {
            StopAreas = stopAreas;
            Borders = borders;
            Particles = particles;
            AccelerationAreas = accelerationAreas;
        }

        public void Move(TimeSpan deltaTime)
        {
            var stoppedParticles = Particles.Where(p => p.IsStopped()).ToArray();
            var particles = Particles.Where(p => !p.IsStopped()).ToArray();

            //foreach (var particle in particles)
            Parallel.ForEach(Particles, particle =>
                {
                    var initialPosition = particle.Position;

                    // Check if stopped
                    if (StopAreas.Any(a => a.IsInRange(particle)))
                    {
                        particle.Stop();
                        return;
                    }

                    // Accelerate within specified areas
                    foreach (var area in AccelerationAreas.Where(a => a.IsInRange(particle)))
                    {
                        particle.SetAcceleration(area.GetAcceleration(particle));
                    }

                    particle.Move(deltaTime);

                    var newPosition = particle.Position;

                    // Check for collision with borders
                    foreach (var border in Borders)
                    {
                        var side1 = border.GetSide(initialPosition);
                        var side2 = border.GetSide(newPosition);
                        if (side1 != side2 && border.IsVectorCrossingBorder(initialPosition, newPosition))
                        {
                            particle.UndoLastMove();
                            particle.Bump(border.CollisionVector);

                            // Bump with one border at a time
                            return;
                        }
                    }

                    // Check for collision with stopeed particles
                    var count =
                            stoppedParticles.Count(p => (p.Position - particle.Position).GetLength() < 7);


                    if (count > 3)
                    {
                        particle.Stop();
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
}