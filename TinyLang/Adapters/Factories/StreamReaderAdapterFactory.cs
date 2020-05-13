using System;
using System.Collections.Generic;
using System.Text;

namespace TinyLang.Adapters.Factories
{
    public interface IStreamReaderAdapterFactory
    {
        IStreamReaderAdapter BuildStreamReaderAdapater(string input);
    }
    public class StreamReaderAdapterFactory : IStreamReaderAdapterFactory
    {
        public IStreamReaderAdapter BuildStreamReaderAdapater(string input)
        {
            return new StreamReaderAdapter(input);
        }
    }
}
