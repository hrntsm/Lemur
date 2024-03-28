using Xunit;

namespace Lemur.Mesh.Element.Tests
{
    public class Hex361Tests
    {
        [Fact]
        public void ConstructElementTest()
        {
            var element = new Hex361([1, 2, 3, 4, 5, 6, 7, 8]);
            Assert.Equal(LeElementType.Hex361, element.ElementType);
            Assert.Equal([1, 2, 3, 4, 5, 6, 7, 8], element.NodeIds);
        }

        [Fact]
        public void ConstructElementWithInvalidNodeLengthTest()
        {
            Assert.Throws<ArgumentException>(() => new Hex361([1, 2, 3]));
        }

        [Fact]
        public void GetSurfaceIdTest()
        {
            var element = new Hex361([1, 2, 3, 4, 5, 6, 7, 8]);
            Assert.Equal(1, element.GetSurfaceId([1, 2, 3, 4]));
            Assert.Equal(2, element.GetSurfaceId([5, 6, 7, 8]));
            Assert.Equal(3, element.GetSurfaceId([1, 2, 5, 6]));
            Assert.Equal(4, element.GetSurfaceId([2, 3, 6, 7]));
            Assert.Equal(5, element.GetSurfaceId([3, 4, 8, 7]));
            Assert.Equal(6, element.GetSurfaceId([1, 4, 8, 5]));
        }

        [Fact]
        public void GetSurfaceIdWithInvalidNodeLengthTest()
        {
            var element = new Hex361([1, 2, 3, 4, 5, 6, 7, 8]);
            Assert.Throws<ArgumentException>(() => element.GetSurfaceId([1, 2]));
        }

        [Fact]
        public void GetSurfaceIdWithInvalidSurfaceNodesTest()
        {
            var element = new Hex361([1, 2, 3, 4, 5, 6, 7, 8]);
            Assert.Throws<ArgumentException>(() => element.GetSurfaceId([1, 2, 9, 10]));
        }

        [Fact]
        public void GetSurfaceIdWithInvalidSurfaceNodeOrderTest()
        {
            var element = new Hex361([1, 2, 3, 4, 5, 6, 7, 8]);
            Assert.Throws<ArgumentException>(() => element.GetSurfaceId([2, 3, 3, 4]));
        }

        [Fact]
        public void ToMshTest()
        {
            var element = new Hex361([1, 2, 3, 4, 5, 6, 7, 8]);
            string expected = "        1,        1,        2,        3,        4,        5,        6,        7,        8";
            Assert.Equal(expected, element.ToMsh(1));
        }
    }
}
