using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoSharp.Core
{
    public struct MessageTag
    {
        public MessageTag(int tag) 
        {
            _tag = tag;
        }

        public int Tag { get { return _tag >> 3; } }
        public WireType WireType { get { return (WireType)(_tag) & WireType.Mask; } }

        int _tag;
    }
}
