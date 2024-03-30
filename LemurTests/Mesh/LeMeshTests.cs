using Lemur.Mesh.Element;
using Lemur.Mesh.Group;

using Xunit;

namespace Lemur.Mesh
{
    public class LeMeshTests
    {
        [Fact]
        public void AddNodeTest()
        {
            var mesh = new LeMesh("Test");
            var node = new LeNode(1, 0.0, 0.0, 0.0);

            mesh.AddNode(node);
            Assert.Single(mesh.Nodes);
            Assert.Equal(node, mesh.Nodes[0]);
        }

        [Fact]
        public void AddElementTest()
        {
            var mesh = new LeMesh("Test");
            var nodes = new LeNodeList
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
            var mesh = new LeMesh("Test");
            var element = new Tetra341([1, 2, 3, 5]);
            Assert.Throws<ArgumentException>(() => mesh.AddElement(element));
        }

        [Fact]
        public void ToMshTest()
        {
            var mesh = new LeMesh("Test");
            var nodes = new LeNodeList
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
                mesh.Elements[0].ToMsh(1) +
                "!END\r\n";
            Assert.Equal(expected, mesh.ToMsh());
        }

        [Fact]
        public void SerializeTest()
        {
            var mesh = new LeMesh("Test");
            var nodes = new LeNodeList
            {
                new(1, 0.0, 0.0, 0.0),
                new(2, 1.0, 0.0, 0.0),
                new(3, 1.0, 1.0, 0.0),
                new(4, 0.0, 1.0, 0.0),
                new(5, 0.0, 0.0, 1.0),
                new(6, 1.0, 0.0, 1.0),
                new(7, 1.0, 1.0, 1.0),
                new(8, 0.0, 1.0, 1.0),
                new(9, 0.5, 0.5, 0.5),
                new(10, 0.5, 0.5, 1.0),
                new(11, 0.5, 0.5, 0.0),
                new(12, 0.5, 0.0, 0.5),
            };
            var tetra = new Tetra341([1, 2, 3, 4]);
            var hex = new Hex361([1, 2, 3, 4, 5, 6, 7, 8]);

            mesh.AddNodes(nodes);
            mesh.AddElement(tetra);
            mesh.AddElement(hex);

            mesh.AddGroup(new NGroup("ng1", [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]));
            mesh.AddGroup(new EGroup("eg1", [1, 2]));
            mesh.AddGroup(new SGroup("sg1", [(1, 1), (2, 2)]));

            mesh.Serialize("./test.msh");
        }
    }
}
