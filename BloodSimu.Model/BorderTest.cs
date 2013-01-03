using NUnit.Framework;

namespace BloodSimu.Model
{
    public class BorderTest
    {
        [Test]
        public void ShouldReturnPointSide()
        {
            // given
            var border = new Border(new Vector2D(0, 0), new Vector2D(1, 0));
            var topPoint = new Vector2D(0.5, 0.5);
            var bottomPoint = new Vector2D(0.5, -0.5);

            // when
            int topSide = border.GetSide(topPoint);
            int bottomSide = border.GetSide(bottomPoint);

            // then
            Assert.That(topSide * bottomSide, Is.LessThan(0));
        }

        [Test]
        public void ShouldReturnPointSideWhenVertical()
        {
            // given
            var border = new Border(new Vector2D(0, 0), new Vector2D(0, 1));
            var leftPoint = new Vector2D(-0.5, 0.5);
            var rightPoint = new Vector2D(0.5, 0.5);

            // when
            int leftSide = border.GetSide(leftPoint);
            int rightSide = border.GetSide(rightPoint);

            // then
            Assert.That(leftSide * rightSide, Is.LessThan(0));
        }

        [Test]
        public void ShouldReturnPointSideWhenDiagonal()
        {
            // given
            var border = new Border(new Vector2D(200, 0), new Vector2D(350, 150));
            var leftPoint = new Vector2D(200, 10);
            var rightPoint = new Vector2D(400, 10);

            // when
            int leftSide = border.GetSide(leftPoint);
            int rightSide = border.GetSide(rightPoint);

            // then
            Assert.That(leftSide * rightSide, Is.LessThan(0));
        }

        [Test]
        public void ShouldCalculateIntersectionPoint()
        {
            // given
            var border = new Border(new Vector2D(0, 0), new Vector2D(1, 1));

            // when
            var intersectionPoint = border.GetIntersectionPoint(new Vector2D(1, 0), new Vector2D(0, 1));

            // then
            Assert.That(intersectionPoint, Is.EqualTo(new Vector2D(0.5, 0.5)));
        }

        [Test]
        public void ShouldCalculateVerticalIntersectionPoint()
        {
            // given
            var border = new Border(new Vector2D(0, 0), new Vector2D(2, 0));

            // when
            var intersectionPoint = border.GetIntersectionPoint(new Vector2D(1, -1), new Vector2D(1, 1));

            // then
            Assert.That(intersectionPoint, Is.EqualTo(new Vector2D(1, 0)));
        }

        [Test]
        public void ShouldCalculateHorizontalIntersectionPoint()
        {
            // given
            var border = new Border(new Vector2D(0, 0), new Vector2D(0, 2));

            // when
            var intersectionPoint = border.GetIntersectionPoint(new Vector2D(-1, 1), new Vector2D(1, 1));

            // then
            Assert.That(intersectionPoint, Is.EqualTo(new Vector2D(0, 1)));
        }

        [Test]
        public void ShouldDetermineIfParticleCrossesBorder()
        {
            // given
            var border = new Border(new Vector2D(2, 2), new Vector2D(4, 3));
            var start = new Vector2D(0, 0);
            var end = new Vector2D(0, 5);

            // when
            var result = border.IsVectorCrossingBorder(start, end);

            // then
            Assert.That(result, Is.False);
        }

        [Test]
        public void ShouldDetermineIfParticleCrossesVerticalBorder()
        {
            // given
            var border = new Border(new Vector2D(0, 0), new Vector2D(0, 4));
            var start = new Vector2D(-3, -3);
            var end = new Vector2D(5, -5);

            // when
            var result = border.IsVectorCrossingBorder(start, end);
            
            // then
            Assert.That(result, Is.False);
        }

        [Test]
        public void ShouldDetermineIfParticleCrossesWhenDiagonal()
        {
            // given
            var border = new Border(new Vector2D(200, 0), new Vector2D(350, 150));
            var leftPoint = new Vector2D(200, 10);
            var rightPoint = new Vector2D(400, 10);

            // when
            int leftSide = border.GetSide(leftPoint);
            int rightSide = border.GetSide(rightPoint);

            var result = border.IsVectorCrossingBorder(leftPoint, rightPoint);

            // then
            Assert.That(result, Is.False);
        }
    }
}