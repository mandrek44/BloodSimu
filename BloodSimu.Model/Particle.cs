﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodSimu.Model
{
    public class Particle
    {
        public Particle(Vector2D startingPosition)
        {
            Position = startingPosition;
            Velocity = Vector2D.Zero;
            Acceleration = Vector2D.Zero;
        }

        public Particle(Vector2D startingPosition, Vector2D startingSpeed)
            : this(startingPosition)
        {
            Velocity = startingSpeed;
        }

        public Particle(Vector2D startingPosition, Vector2D startingSpeed, Vector2D startingAcceleration)
            : this(startingPosition, startingSpeed)
        {
            Acceleration = startingAcceleration;
        }

        public Vector2D Position { get; private set; }
        public Vector2D Velocity { get; private set; }
        public Vector2D Acceleration { get; private set; }

        public void Move(TimeSpan deltaTime)
        {
            var delta = deltaTime.TotalSeconds;
            Velocity = Velocity + (Acceleration * delta);
            Position = Position + Velocity * delta;
        }

        public void BumpPlane(Vector2D collisionNormalPlane)
        {
            collisionNormalPlane = collisionNormalPlane.Normalize();
            var collisionNormalVelocity = new Vector2D(collisionNormalPlane.Dot(Velocity), collisionNormalPlane.Perpendicular().Dot(Velocity));
            
            var afterCollisionNormalVelocity = new Vector2D(-collisionNormalVelocity.X, collisionNormalVelocity.Y);

            Velocity = collisionNormalPlane*afterCollisionNormalVelocity.X + collisionNormalPlane.Perpendicular()*afterCollisionNormalVelocity.Y;
        }

        public void SetAcceleration(Vector2D newAcceleration)
        {
            Acceleration = newAcceleration;
        }

        public override string ToString()
        {
            return string.Format("Position: {0}, Velocity: {1}", Position, Velocity);
        }
    }
}