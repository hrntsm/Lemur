using Xunit;

namespace Fistr.Core.Mesh.Element.Tests
{
    public class Tetra341Tests
    {
        [Fact]
        public void ConstructElementTest()
        {
            var element = new Tetra341([1, 2, 3, 4]);
            Assert.Equal(FistrElementType.Tetra341, element.ElementType);
            Assert.Equal([1, 2, 3, 4], element.NodeIds);
        }

        [Fact]
        public void ConstructElementWithInvalidNodeLengthTest()
        {
            Assert.Throws<ArgumentException>(() => new Tetra341([1, 2, 3]));
        }

        [Fact]
        public void GetSurfaceIdTest()
        {
            var element = new Tetra341([1, 2, 3, 4]);
            Assert.Equal(1, element.GetSurfaceId([1, 2, 3]));
            Assert.Equal(1, element.GetSurfaceId([1, 3, 2]));
            Assert.Equal(1, element.GetSurfaceId([2, 1, 3]));
            Assert.Equal(1, element.GetSurfaceId([2, 3, 1]));
            Assert.Equal(1, element.GetSurfaceId([3, 1, 2]));
            Assert.Equal(1, element.GetSurfaceId([3, 2, 1]));

            Assert.Equal(2, element.GetSurfaceId([1, 2, 4]));
            Assert.Equal(3, element.GetSurfaceId([2, 3, 4]));
            Assert.Equal(4, element.GetSurfaceId([1, 3, 4]));
        }

        [Fact]
        public void GetSurfaceIdWithInvalidNodeLengthTest()
        {
            var element = new Tetra341([1, 2, 3, 4]);
            Assert.Throws<ArgumentException>(() => element.GetSurfaceId([1, 2]));
        }

        [Fact]
        public void GetSurfaceIdWithInvalidSurfaceNodesTest()
        {
            var element = new Tetra341([1, 2, 3, 4]);
            Assert.Throws<ArgumentException>(() => element.GetSurfaceId([1, 2, 5]));
        }

        [Fact]
        public void GetSurfaceIdWithInvalidSurfaceNodeOrderTest()
        {
            var element = new Tetra341([1, 2, 3, 4]);
            Assert.Throws<ArgumentException>(() => element.GetSurfaceId([2, 3, 3]));
        }

        [Fact]
        public void ToMshTest()
        {
            var element1 = new Tetra341([1, 2, 3, 4]);
            string expected1 = "        1,        1,        2,        3,        4";
            Assert.Equal(expected1, element1.ToMsh(1));

            var element2 = new Tetra341([10, 2000, 30000, 400000]);
            string expected2 = "       10,       10,     2000,    30000,   400000";
            Assert.Equal(expected2, element2.ToMsh(10));
        }
    }
}
