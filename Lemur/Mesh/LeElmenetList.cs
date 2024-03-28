using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Lemur.Mesh.Element;

namespace Lemur.Mesh
{
    public class LeElementList : IList<LeElementBase>
    {
        public LeElementType ElementType { get; }
        public bool IsReadOnly => true;
        public int Count => _elements.Count;
        public LeElementBase this[int index]
        {
            get => _elements[index];
            set => _elements[index] = value;
        }

        private readonly List<LeElementBase> _elements;

        public LeElementList(LeElementType elementType)
        {
            ElementType = elementType;
            _elements = new List<LeElementBase>();
        }

        public void AddElement(LeElementBase element)
        {
            if (element.ElementType != ElementType)
            {
                throw new ArgumentException("Element type mismatch.");
            }
            _elements.Add(element);
        }

        public string ToMsh(int startId)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"!ELEMENT, TYPE={(int)ElementType}, EGRP={ElementType + "_AUTO"}");
            for (int i = 0; i < _elements.Count; i++)
            {
                LeElementBase element = _elements[i];
                sb.AppendLine(element.ToMsh(startId + i));
            }

            return sb.ToString();
        }

        public int IndexOf(LeElementBase item)
        {
            return _elements.IndexOf(item);
        }

        public void Insert(int index, LeElementBase item)
        {
            if (item.ElementType != ElementType)
            {
                throw new ArgumentException("Element type mismatch.");
            }
            _elements.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _elements.RemoveAt(index);
        }

        public void Add(LeElementBase item)
        {
            AddElement(item);
        }

        public void Clear()
        {
            _elements.Clear();
        }

        public bool Contains(LeElementBase item)
        {
            return _elements.Contains(item);
        }

        public void CopyTo(LeElementBase[] array, int arrayIndex)
        {
            _elements.CopyTo(array, arrayIndex);
        }

        public bool Remove(LeElementBase item)
        {
            return _elements.Remove(item);
        }

        public IEnumerator<LeElementBase> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
