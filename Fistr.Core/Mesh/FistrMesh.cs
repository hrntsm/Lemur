using System.Collections.Generic;

namespace Fistr.Core.Mesh
{
    public class FistrMesh
    {
        public FistrElementType ElementType;
        public List<FistrNode> Nodes;

        public FistrMesh(FistrElementType meshType, List<FistrNode> nodes)
        {
            ElementType = meshType;
            Nodes = nodes;
        }
    }
}
