using System;
using System.Runtime.Serialization;

namespace Http.Consumer.Exceptions
{
    public class HttpReponseException : Exception
    {
        public HttpReponseException()
        {
        }

        public HttpReponseException(string message) : base(message)
        {
        }

        public HttpReponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpReponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
