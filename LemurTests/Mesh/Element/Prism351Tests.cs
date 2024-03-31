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
        public void ToMshTest()
        {
            var element = new Prism351([1, 2, 3, 4, 5, 6]);
            string expected = "        1,        1,        2,        3,        4,        5,        6";
            Assert.Equal(expected, element.ToMsh(1));
        }
    }
}
