using Fistr.Core.Mesh.Element;

using Xunit;

namespace Fistr.Core.Mesh
{
    public class FistrMeshTests
    {
        [Fact]
        public void AddNodeTest()
        {
            var mesh = new FistrMesh("Test");
            var node = new FistrNode(1, 0.0, 0.0, 0.0);

            mesh.AddNode(node);
            Assert.Single(mesh.Nodes);
            Assert.Equal(node, mesh.Nodes[0]);
        }

        [Fact]
        public void AddElementTest()
        {
            var mesh = new FistrMesh("Test");
            var nodes = new FistrNodeList
            {
                new(1, 0.0, 0.0, 0.0),
                new(2, 1.0, 0.0, 0.0),
                new(3, 1.0, 1.0, 0.0),
                new(4, 0.0, 1.0, 0.0)
            };
            var element = new Tetra341([1, 2, 3, 4]);

            mesh.AddNodes(nodes);
            mesh.AddElement(element);
            Assert.Single(mesh.Elements);
            Assert.Single(mesh.Elements[0]);
            Assert.Equal(element, mesh.Elements[0][0]);
        }

        [Fact]
        public void AddElementNodeNotExistTest()
        {
            var mesh = new FistrMesh("Test");
            var element = new Tetra341([1, 2, 3, 5]);
            Assert.Throws<ArgumentException>(() => mesh.AddElement(element));
        }

        [Fact]
        public void ToMshTest()
        {
            var mesh = new FistrMesh("Test");
            var nodes = new FistrNodeList
            {
                new(1, 0.0, 0.0, 0.0),
                new(2, 1.0, 0.0, 0.0),
                new(3, 1.0, 1.0, 0.0),
                new(4, 0.0, 1.0, 0.0)
            };
            var element = new Tetra341([1, 2, 3, 4]);

            mesh.AddNodes(nodes);
            mesh.AddElement(element);

            string expected =
                "!HEADER\r\n" +
                " Test\r\n" +
                mesh.Nodes.ToMsh() +
                mesh.Elements[0].ToMsh(1);
            Assert.Equal(expected, mesh.ToMsh());
        }

        [Fact]
        public void SerializeTest()
        {
            var mesh = new FistrMesh("Test");
            var nodes = new FistrNodeList
            {
                new(1, 0.0, 0.0, 0.0),
                new(2, 1.0, 0.0, 0.0),
                new(3, 1.0, 1.0, 0.0),
                new(4, 0.0, 1.0, 0.0)
            };
            var element = new Tetra341([1, 2, 3, 4]);

            mesh.AddNodes(nodes);
            mesh.AddElement(element);

            mesh.Serialize("./test.msh");
        }
    }
}
