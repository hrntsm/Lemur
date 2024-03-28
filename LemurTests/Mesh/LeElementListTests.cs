using Xunit;

namespace Lemur.Mesh.Element.Tests
{
    public class LeElementListTests
    {
        [Fact]
        public void ConstructElementListTest()
        {
            var elementList = new LeElementList(LeElementType.Tetra341);
            Assert.Equal(LeElementType.Tetra341, elementList.ElementType);
            Assert.Empty(elementList);
        }

        [Fact]
        public void AddElementTest()
        {
            var elementList = new LeElementList(LeElementType.Tetra341);
            var element = new Tetra341([1, 2, 3, 4]);
            elementList.AddElement(element);
            Assert.Single(elementList);
            Assert.Equal(element, elementList[0]);
        }

        [Fact]
        public void AddElementWithMismatchElementTypeTest()
        {
            var elementList = new LeElementList(LeElementType.Tetra341);
            var element = new Hex361([1, 2, 3, 4, 5, 6, 7, 8]);
            Assert.Throws<ArgumentException>(() => elementList.AddElement(element));
        }

        [Fact]
        public void ToMshTest()
        {
            var elementList = new LeElementList(LeElementType.Tetra341);
            elementList.AddElement(new Tetra341([1, 2, 3, 4]));
            elementList.AddElement(new Tetra341([5, 6, 7, 8]));
            string expected =
                "!ELEMENT, TYPE=341, EGRP=Tetra341_AUTO\r\n" +
                "        1,        1,        2,        3,        4\r\n" +
                "        2,        5,        6,        7,        8\r\n";
            Assert.Equal(expected, elementList.ToMsh(1));
        }
    }
}
