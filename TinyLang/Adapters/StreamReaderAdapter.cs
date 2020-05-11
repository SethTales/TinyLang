using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TinyLang.Adapters
{
    public interface IStreamReaderAdapter : IDisposable
    {
        int Read();
        int Peek();
    }

    public class StreamReaderAdapter : IStreamReaderAdapter
    {
        private readonly StreamReader _streamReader;

        public StreamReaderAdapter(string input)
        {
            _streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(input)));
        }

        public int Read()
        {
            return _streamReader.Read();
        }

        public int Peek()
        {
            return _streamReader.Peek();
        }
        public void Dispose()
        {
            _streamReader.Dispose();
        }
    }
}
