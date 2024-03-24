using System.Text;

using Xunit;

namespace Fistr.Core.Mesh.Tests
{
    public class FistrNodeListTests
    {
        [Fact]
        public void AddNodeTest()
        {
            var nodeList = new FistrNodeList();
            nodeList.AddNode(1, 0, 0, 0);
            Assert.Single(nodeList.Nodes);
        }

        [Fact]
        public void AddNodeWithoutIdTest()
        {
            var nodeList = new FistrNodeList();
            nodeList.AddNode(100, 0, 0, 0);
            nodeList.AddNode(0, 0, 0);
            Assert.Equal(2, nodeList.Nodes.Count);
            Assert.Equal(101, nodeList.Nodes[1].Id);
        }

        [Fact]
        public void AddNodeRangeTest()
        {
            var nodeList = new FistrNodeList();
            FistrNode[] nodes =
            [
                new FistrNode(1, 0, 0, 0),
                new FistrNode(2, 1, 0, 0),
                new FistrNode(3, 1, 1, 0)
            ];
            nodeList.AddNodeRange(nodes);
            Assert.Equal(3, nodeList.Nodes.Count);
        }

        [Fact]
        public void AddNodeRangeExceptionTest()
        {
            var nodeList = new FistrNodeList();
            FistrNode[] nodes =
            [
                new FistrNode(1, 0, 0, 0),
                new FistrNode(1, 1, 0, 0)
            ];
            Assert.Throws<ArgumentException>(() => nodeList.AddNodeRange(nodes));
        }

        [Fact]
        public void AddNegativeIdNodeExceptionTest()
        {
            var nodeList = new FistrNodeList();
            Assert.Throws<ArgumentException>(() => nodeList.AddNode(0, 1, 0, 0));
            Assert.Throws<ArgumentException>(() => nodeList.AddNode(-1, 1, 0, 0));
        }

        [Fact]
        public void ToMshTest()
        {
            var nodeList = new FistrNodeList();
            FistrNode[] nodes =
            [
                new FistrNode(1, 0, 0, 0),
                new FistrNode(2, 1, 0, 0),
                new FistrNode(3, 1, 1, 0)
            ];
            nodeList.AddNodeRange(nodes);
            string msh = nodeList.ToMsh();
            var expected = new StringBuilder();
            expected.AppendLine("!NODE");
            expected.AppendLine(nodes[0].ToMsh());
            expected.AppendLine(nodes[1].ToMsh());
            expected.AppendLine(nodes[2].ToMsh());
            Assert.Equal(expected.ToString(), msh);
        }
    }
}
