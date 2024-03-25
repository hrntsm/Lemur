using System.Text;

using Xunit;

namespace Fistr.Core.Mesh.Tests
{
    public class FistrNodeListTests
    {
        [Fact]
        public void AddNodeTest()
        {
            var nodeList = new FistrNodeList
            {
                { 1, 0, 0, 0 }
            };
            Assert.Single(nodeList);
        }

        [Fact]
        public void AddNodeWithoutIdTest()
        {
            var nodeList = new FistrNodeList
            {
                { 100, 0, 0, 0 },
                { 0, 0, 0 }
            };
            Assert.Equal(2, nodeList.Count);
            Assert.Equal(101, nodeList[1].Id);
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
            nodeList.AddRange(nodes);
            Assert.Equal(3, nodeList.Count);
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
            Assert.Throws<ArgumentException>(() => nodeList.AddRange(nodes));
        }

        [Fact]
        public void AddNegativeIdNodeExceptionTest()
        {
            var nodeList = new FistrNodeList();
            Assert.Throws<ArgumentException>(() => nodeList.Add(0, 1, 0, 0));
            Assert.Throws<ArgumentException>(() => nodeList.Add(-1, 1, 0, 0));
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
            nodeList.AddRange(nodes);
            string msh = nodeList.ToMsh();
            var expected = new StringBuilder();
            expected.AppendLine("!NODE, NGRP=NGRP_AUTO");
            expected.AppendLine(nodes[0].ToMsh());
            expected.AppendLine(nodes[1].ToMsh());
            expected.AppendLine(nodes[2].ToMsh());
            Assert.Equal(expected.ToString(), msh);
        }
    }
}
