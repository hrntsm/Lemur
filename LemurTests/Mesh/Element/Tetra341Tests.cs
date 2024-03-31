using Xunit;

namespace Lemur.Mesh.Element.Tests
{
    public class Tetra341Tests
    {
        [Fact]
        public void ConstructElementTest()
        {
            var element = new Tetra341([1, 2, 3, 4]);
            Assert.Equal(LeElementType.Tetra341, element.ElementType);
            Assert.Equal([1, 2, 3, 4], element.NodeIds);
        }

        [Fact]
        public void ConstructElementWithInvalidNodeLengthTest()
        {
            Assert.Throws<ArgumentException>(() => new Tetra341([1, 2, 3]));
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
