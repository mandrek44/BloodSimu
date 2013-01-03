namespace BloodSimu.Model
{
    public class StopArea
    {
        public Vector2D Center { get; private set; }

        public double Radius { get; private set; }

        public StopArea(Vector2D center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool IsInRange(Particle particle)
        {
            return (Center - particle.Position).GetLength() < Radius;
        }
    }
}