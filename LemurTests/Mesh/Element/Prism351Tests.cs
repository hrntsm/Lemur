using Xunit;

namespace Lemur.Mesh.Element.Tests
{
    public class Prism351Tests
    {
        [Fact]
        public void ConstructElementTest()
        {
            var element = new Prism351([1, 2, 3, 4, 5, 6]);
            Assert.Equal(LeElementType.Prism351, element.ElementType);
            Assert.Equal([1, 2, 3, 4, 5, 6], element.NodeIds);
        }

        [Fact]
        public void ConstructElementWithInvalidNodeLengthTest()
        {
            Assert.Throws<ArgumentException>(() => new Prism351([1, 2, 3]));
        }

        [Fact]
        public void GetSurfaceIdTriTest()
        {
            var element = new Prism351([1, 2, 3, 4, 5, 6]);
            Assert.Equal(1, element.GetSurfaceId([1, 2, 3]));
            Assert.Equal(2, element.GetSurfaceId([4, 5, 6]));
        }

        [Fact]
        public void GetSurfaceIdQuadTest()
        {
            var element = new Prism351([1, 2, 3, 4, 5, 6]);
            Assert.Equal(3, element.GetSurfaceId([1, 2, 4, 5]));
            Assert.Equal(4, element.GetSurfaceId([2, 3, 5, 6]));
            Assert.Equal(5, element.GetSurfaceId([1, 3, 4, 6]));
        }

        [Fact]
        public void GetSurfaceIdWithInvalidNodeLengthTest()
        {
            var element = new Prism351([1, 2, 3, 4, 5, 6]);
            Assert.Throws<ArgumentException>(() => element.GetSurfaceId([1, 2]));
        }

        [Fact]
        public void GetSurfaceIdWithInvalidSurfaceNodesTest()
        {
            var element = new Prism351([1, 2, 3, 4, 5, 6]);
            Assert.Throws<ArgumentException>(() => element.GetSurfaceId([1, 2, 7]));
        }

        [Fact]
        public void GetSurfaceIdWithInvalidSurfaceNodeOrderTest()
        {
            var element = new Prism351([1, 2, 3, 4, 5, 6]);
            Assert.Throws<ArgumentException>(() => element.GetSurfaceId([3, 4, 5]));
            Assert.Throws<ArgumentException>(() => element.GetSurfaceId([3, 4, 5, 6]));
        }

        [Fact]
        public void ToMshTest()
        {
            var element = new Prism351([1, 2, 3, 4, 5, 6]);
            string expected = "        1,        1,        2,        3,        4,        5,        6";
            Assert.Equal(expected, element.ToMsh(1));
        }
    }
}
