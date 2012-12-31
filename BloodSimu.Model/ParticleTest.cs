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

        [TestCase(-1, 0, 2, 0, 1, 0)]
        [TestCase(2, 0, 2, 0, -2, 0)]
        [TestCase(0, -1, 0, 2, 0, 1)]
        [TestCase(1, 1, -1, -1, -1, -1)]
        public void ShouldBump(double startVelocityX, double startVelocityY, double collisionPlaneX, double collistionPlaneY, double resultVelocityX, double resultVelocityY)
        {
            // given
            var particle = new Particle(Vector2D.Zero, new Vector2D(startVelocityX, startVelocityY));
            var collisionNormalPlane = new Vector2D(collisionPlaneX, collistionPlaneY);

            // when
            particle.Bump(collisionNormalPlane);

            // then
            Assert.That((particle.Velocity - new Vector2D(resultVelocityX, resultVelocityY)).GetLength() < 0.00000001);
        }
    }
}