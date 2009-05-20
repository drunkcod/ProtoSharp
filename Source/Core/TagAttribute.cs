namespace ProtoSharp.Core
{  
    using System;

    public sealed class TagAttribute : Attribute
    {
        int number;

        public TagAttribute(int number)
        {
            this.number = number;
        }

        public int Number { get { return number; } }
        public bool UseFixed { get;  set; }
        public bool UseZigZag { get; set; }
        public bool Packed { get; set; }
    }
}
