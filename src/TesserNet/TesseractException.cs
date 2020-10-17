using System;
using System.Runtime.Serialization;

namespace TesserNet
{
    /// <summary>
    /// Exception thrown when something goes wrong with Tesseract execution.
    /// </summary>
    /// <seealso cref="Exception" />
    public class TesseractException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractException"/> class.
        /// </summary>
        public TesseractException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TesseractException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public TesseractException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.</param>
        protected TesseractException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
