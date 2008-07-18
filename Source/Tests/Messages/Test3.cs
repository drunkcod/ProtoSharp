using ProtoSharp.Core;

namespace ProtoSharp.Tests.Messages
{
    class Test3
    {
        [Tag(3)]
        public Test1 C { get { return _c; } set { _c = value; } }

        Test1 _c;
    }
}
