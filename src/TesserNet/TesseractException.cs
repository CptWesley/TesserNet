using System;
using System.Runtime.Serialization;

namespace TesserNet
{
    public class TesseractException : Exception
    {
        public TesseractException()
        {
        }

        public TesseractException(string message)
            : base(message)
        {
        }

        public TesseractException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected TesseractException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
