using System.Collections.Generic;
using System.Text;

using Fistr.Core.Mesh.Element;

namespace Fistr.Core.Mesh
{
    public class FistrElementList
    {
        public FistrElementType ElementType { get; }
        public FistrElementBase[] Elements
        {
            get
            {
                return _elements.ToArray();
            }
        }
        public int Count
        {
            get
            {
                return _elements.Count;
            }
        }

        private readonly List<FistrElementBase> _elements;

        public FistrElementList(FistrElementType elementType)
        {
            ElementType = elementType;
            _elements = new List<FistrElementBase>();
        }

        public void AddElement(FistrElementBase element)
        {
            if (element.ElementType != ElementType)
            {
                throw new System.ArgumentException("Element type mismatch.");
            }
            _elements.Add(element);
        }

        public string ToMsh(int startId)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!ELEMENT, TYPE={(int)ElementType}, NAME={ElementType + "_AUTO"}");
            for (int i = 0; i < _elements.Count; i++)
            {
                FistrElementBase element = _elements[i];
                sb.AppendLine(element.ToMsh(startId + i));
            }

            return sb.ToString();
        }
    }
}
