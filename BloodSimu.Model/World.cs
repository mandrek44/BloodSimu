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

        public World(Collection<Border> borders, Collection<Particle> particles, Collection<AccelerationArea> accelerationAreas, Collection<StopArea> stopAreas )
        {
            StopAreas = stopAreas;
            Borders = borders;
            Particles = particles;
            AccelerationAreas = accelerationAreas;
        }

        public void Move(TimeSpan deltaTime)
        {
            foreach (var particle in Particles.Where(p=>!p.IsStopped()))
            {
                var initialPosition = particle.Position;

                // Check if stopped
                if(StopAreas.Any(a=>a.IsInRange(particle)))
                {
                    particle.Stop();
                    break;
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
                        break;
                    }
                }

                
            }

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