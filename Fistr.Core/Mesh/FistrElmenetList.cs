using System.Collections;
using System.Collections.Generic;
using System.Text;

using Fistr.Core.Mesh.Element;

namespace Fistr.Core.Mesh
{
    public class FistrElementList : IList<FistrElementBase>
    {
        public FistrElementType ElementType { get; }
        public bool IsReadOnly => true;
        public int Count => _elements.Count;
        public FistrElementBase this[int index]
        {
            get => _elements[index];
            set => _elements[index] = value;
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
            sb.AppendLine($"!ELEMENT, TYPE={(int)ElementType}, EGRP={ElementType + "_AUTO"}");
            for (int i = 0; i < _elements.Count; i++)
            {
                FistrElementBase element = _elements[i];
                sb.AppendLine(element.ToMsh(startId + i));
            }

            return sb.ToString();
        }

        public int IndexOf(FistrElementBase item)
        {
            return _elements.IndexOf(item);
        }

        public void Insert(int index, FistrElementBase item)
        {
            if (item.ElementType != ElementType)
            {
                throw new System.ArgumentException("Element type mismatch.");
            }
            _elements.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _elements.RemoveAt(index);
        }

        public void Add(FistrElementBase item)
        {
            AddElement(item);
        }

        public void Clear()
        {
            _elements.Clear();
        }

        public bool Contains(FistrElementBase item)
        {
            return _elements.Contains(item);
        }

        public void CopyTo(FistrElementBase[] array, int arrayIndex)
        {
            _elements.CopyTo(array, arrayIndex);
        }

        public bool Remove(FistrElementBase item)
        {
            return _elements.Remove(item);
        }

        public IEnumerator<FistrElementBase> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
