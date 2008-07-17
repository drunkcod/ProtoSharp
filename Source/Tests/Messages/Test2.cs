using ProtoSharp.Core;

namespace ProtoSharp.Tests.Messages
{
    class Test2
    {
        [Tag(2)]
        public string B { get { return _b; } set { _b = value; } }

        string _b;
    }
}
