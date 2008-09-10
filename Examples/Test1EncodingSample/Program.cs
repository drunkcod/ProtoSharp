using System;
using System.IO;
using ProtoSharp.Core;

//Based on http://code.google.com/apis/protocolbuffers/docs/encoding.html "A Simple Message"
namespace Test1EncodingSample
{
    class Test1
    {
        [Tag(1)]
        public int A { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var encoded = new MemoryStream();
            
            Serializer.Serialize(encoded, new Test1(){ A = 150});
            Console.Write("Encoded = {");//Expected 08 96 01
            var sep = string.Empty;
            Array.ForEach(encoded.ToArray(), b =>
            {
                Console.Write("{0}{1:x}", sep, b);
                sep = ", ";
            });
            Console.WriteLine("}");

            var decoded = MessageReader.Read<Test1>(encoded.ToArray());
            Console.WriteLine("Decoded = {{A: {0}}}", decoded.A);
        }
    }
}
