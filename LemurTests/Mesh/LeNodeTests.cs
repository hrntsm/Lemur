using Xunit;

namespace Lemur.Mesh.Tests
{
    public class LeNodeTests
    {
        [Fact]
        public void ToMshTest()
        {
            var node = new LeNode(1, 0, -0.6, 0);
            string nodeString = node.ToMsh();
            Assert.Equal("        1,   0.0000000000E+00,  -6.0000000000E-01,   0.0000000000E+00", nodeString);
        }
    }
}
