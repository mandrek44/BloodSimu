using System;
using System.Collections.ObjectModel;

namespace BloodSimu.Model
{
    public class World
    {
        public Collection<Particle> Particles;

        public Collection<Border> Borders;

        public void Move(TimeSpan deltaTime)
        {
            foreach (var particle in Particles)
            {
                // TODO: Collistion detection with border

                var initialPosition = particle.Position;

                particle.Move(deltaTime);

                var newPosition = particle.Position;

                foreach (var border in Borders)
                {
                    if (border.GetSide(initialPosition) != border.GetSide(newPosition) && border.IsVectorCrossingBorder(initialPosition, newPosition))
                    {
                        particle.UndoLastMove();
                        particle.Bump(border.CollisionVector);

                        // TODO: Do the cholersterol magic here
                    }
                }
            }
        }
    }
}