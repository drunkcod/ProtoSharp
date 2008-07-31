using System;
using System.Collections.Generic;
using System.Text;

namespace ProtoSharp.Core
{
    public class UnknownFieldCollection
    {
        public int Count { get { return _fields.Count; } }

        public UnknownField this[int index]
        {
            get { return _fields[index]; }
        }

        public void Add(MessageTag tag, MessageReader reader) 
        {
            Add(UnknownField.Create(tag, reader));
        }

        internal void Add(UnknownField unknown)
        {
            _fields.Add(unknown);
        }

        List<UnknownField> _fields = new List<UnknownField>();
    }
}
