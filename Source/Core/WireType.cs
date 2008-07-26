namespace ProtoSharp.Core
{
    public enum WireType
    {
        Unknown = -1,
        Varint = 0,
        Fixed64 = 1,
        String = 2,
        Fixed32 = 5,
        Mask = 0x7
    }
}
