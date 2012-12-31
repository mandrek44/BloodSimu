using NUnit.Framework;

namespace BloodSimu.Model
{
    public class Vector2DTest
    {
        [Test]
        public void ShouldAddTwoVectors()
        {
            var a = new Vector2D(1, 2);
            var b = new Vector2D(3, 4);

            var sum = a + b;

            Assert.That(sum, Is.EqualTo(new Vector2D(4, 6)));
        }

        [Test]
        public void ShouldSubstractTwoVectors()
        {
            var a = new Vector2D(1, 2);
            var b = new Vector2D(3, 4);

            var sub = a - b;

            Assert.That(sub, Is.EqualTo(new Vector2D(-2, -2)));
        }

        [Test]
        public void ShouldMultiplyVectorByCoeffficient()
        {
            var b = new Vector2D(3, 4);

            var multiply = b * 3.5;

            Assert.That(multiply, Is.EqualTo(new Vector2D(3 * 3.5, 4 * 3.5)));
        }

        [Test]
        public void ShouldDivideVectorByCoeffficient()
        {
            var b = new Vector2D(3, 4);

            var divide = b / 2;

            Assert.That(divide, Is.EqualTo(new Vector2D(1.5, 2)));
        }

        [Test]
        public void ShouldReturnSquaredLength()
        {
            var a = new Vector2D(3, 4);

            var length = a.GetSquaredLength();

            Assert.That(length, Is.EqualTo(25));
        }

        [Test]
        public void ShouldReturnLength()
        {
            var a = new Vector2D(3, 4);

            var length = a.GetLength();

            Assert.That(length, Is.EqualTo(5));
        }
    }
}