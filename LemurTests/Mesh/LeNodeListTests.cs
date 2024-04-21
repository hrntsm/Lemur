using System.Text;

using Xunit;

namespace Lemur.Mesh.Tests
{
    public class LeNodeListTests
    {
        [Fact]
        public void AddNodeTest()
        {
            var nodeList = new LeNodeList
            {
                { 1, 0, 0, 0 }
            };
            Assert.Single(nodeList);
        }

        [Fact]
        public void AddNodeWithoutIdTest()
        {
            var nodeList = new LeNodeList
            {
                { 100, 0, 0, 0 },
            };
            Assert.Equal(2, nodeList.Count);
            Assert.Equal(101, nodeList[1].Id);
        }

        [Fact]
        public void AddNodeRangeTest()
        {
            var nodeList = new LeNodeList();
            LeNode[] nodes =
            [
                new LeNode(1, 0, 0, 0),
                new LeNode(2, 1, 0, 0),
                new LeNode(3, 1, 1, 0)
            ];
            nodeList.AddRange(nodes);
            Assert.Equal(3, nodeList.Count);
        }

        [Fact]
        public void AddNodeRangeExceptionTest()
        {
            var nodeList = new LeNodeList();
            LeNode[] nodes =
            [
                new LeNode(1, 0, 0, 0),
                new LeNode(1, 1, 0, 0)
            ];
            Assert.Throws<ArgumentException>(() => nodeList.AddRange(nodes));
        }

        [Fact]
        public void AddNegativeIdNodeExceptionTest()
        {
            var nodeList = new LeNodeList();
            Assert.Throws<ArgumentException>(() => nodeList.Add(0, 1, 0, 0));
            Assert.Throws<ArgumentException>(() => nodeList.Add(-1, 1, 0, 0));
        }

        [Fact]
        public void ToMshTest()
        {
            var nodeList = new LeNodeList();
            LeNode[] nodes =
            [
                new LeNode(1, 0, 0, 0),
                new LeNode(2, 1, 0, 0),
                new LeNode(3, 1, 1, 0)
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
