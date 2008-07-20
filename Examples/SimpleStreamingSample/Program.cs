using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ProtoSharp.Core;

namespace ProtoSharp.Examples.SimpleStreamingSample
{
    #region Server Part
    public class MyMessage
    {
        [Tag(1)]
        public List<int> Data { get { return _data; } }

        List<int> _data = new List<int>();
    }

    public class MessageServer
    {
        public Stream GetServerStream()
        {
            MyMessage serverData = new MyMessage();
            for(int i = 0; i != 10; ++i)
                serverData.Data.Add(i);

            return new MemoryStream(MessageWriter.Write(serverData));
        }
    }
    #endregion

    #region Client Part
    public interface IDataHandler<T>
    {
        void Add(T value);
        IEnumerator GetEnumerator();
    }

    public class MyMessageStream
    {
        public MyMessageStream(IDataHandler<int> data)
        {
            _data = data;
        }

        [Tag(1)]
        public IDataHandler<int> Data { get { return _data; } }

        IDataHandler<int> _data;
    }

    class Program
    {
        class PrintEvenDataHandler : IDataHandler<int>
        {
            public void Add(int value)
            {
                if(value % 2 == 0)
                    Console.Write("{0} ", value);
            }

            public IEnumerator GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        static void Main(string[] args)
        {
            MyMessageStream stream = new MyMessageStream(new PrintEvenDataHandler());
            MessageServer server = new MessageServer();
            new MessageReader(new ByteReader(server.GetServerStream())).Read(stream);                       
        }
    }
    #endregion
}
