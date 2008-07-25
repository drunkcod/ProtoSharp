
namespace ProtoSharp.Core
{
    public struct MessageTag
    {
        public static int GetNumber(int tag) { return tag >> 3; }
        public static WireType GetWireType(int tag) { return (WireType)tag & WireType.Mask; }

        public MessageTag(int tag) 
        {
            _tag = tag;
        }

        public int Number { get { return GetNumber(_tag); } }
        public WireType WireType { get { return GetWireType(_tag); } }

        int _tag;
    }
}
