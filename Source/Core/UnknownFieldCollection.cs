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
            _fields.Add(UnknownField.Create(tag, reader));
        }

        List<UnknownField> _fields = new List<UnknownField>();
    }
}
