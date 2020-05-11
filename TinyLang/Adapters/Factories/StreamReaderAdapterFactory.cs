using System;
using System.Collections.Generic;
using System.Text;

namespace TinyLang.Adapters.Factories
{
    public interface IStreamReaderAdapterFactory
    {
        IStreamReaderAdapter BuilStreamReaderAdapter(string input);
    }
    public class StreamReaderAdapterFactory : IStreamReaderAdapterFactory
    {
        public IStreamReaderAdapter BuilStreamReaderAdapter(string input)
        {
            return new StreamReaderAdapter(input);
        }
    }
}
