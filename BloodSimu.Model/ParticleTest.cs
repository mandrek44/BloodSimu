using System;
using NUnit.Framework;

namespace BloodSimu.Model
{
    public class ParticleTest
    {
        [Test]
        public void ShouldNotMoveWhenNoVelocity()
        {
            var particle = new Particle(Vector2D.Zero);
            particle.Move(TimeSpan.FromSeconds(1));

            Assert.That(particle.Position, Is.EqualTo(Vector2D.Zero));
        }

        [Test]
        public void ShouldMoveWithConstantSpeedWhenNoAcceleration()
        {
            var particle = new Particle(Vector2D.Zero, new Vector2D(1, 0));
            particle.Move(TimeSpan.FromSeconds(1));

            Assert.That(particle.Position, Is.EqualTo(new Vector2D(1, 0)));
            Assert.That(particle.Velocity, Is.EqualTo(new Vector2D(1, 0)));

            particle.Move(TimeSpan.FromSeconds(1));

            Assert.That(particle.Position, Is.EqualTo(new Vector2D(2, 0)));
            Assert.That(particle.Velocity, Is.EqualTo(new Vector2D(1, 0)));
        }

        [Test]
        public void ShouldAccelerate()
        {
            var particle = new Particle(Vector2D.Zero, Vector2D.Zero, new Vector2D(1, 0));
            
            particle.Move(TimeSpan.FromSeconds(1));

            Assert.That(particle.Position, Is.EqualTo(new Vector2D(1, 0)));
            Assert.That(particle.Velocity, Is.EqualTo(new Vector2D(1, 0)));

            particle.Move(TimeSpan.FromSeconds(1));

            Assert.That(particle.Position, Is.EqualTo(new Vector2D(3, 0)));
            Assert.That(particle.Velocity, Is.EqualTo(new Vector2D(2, 0)));
        }
    }
}