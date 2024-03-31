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
        public void ToMshTest()
        {
            var element = new Hex361([1, 2, 3, 4, 5, 6, 7, 8]);
            string expected = "        1,        1,        2,        3,        4,        5,        6,        7,        8";
            Assert.Equal(expected, element.ToMsh(1));
        }
    }
}
