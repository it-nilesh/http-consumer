using System;
using System.IO;
using System.Net;
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
        public HeaderDictionary ResponseHeader { get; private set; }

        public WebExceptionStatus ExceptionStatus { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        public Lazy<Stream> Content { get; private set; }

        internal void SetResponse(WebException webException)
        {
            var response = webException.Response as HttpWebResponse;
            ResponseHeader = new HeaderDictionary(response.Headers);
            ExceptionStatus = webException.Status;
            StatusCode = response.StatusCode;
            Content = new Lazy<Stream>(() => response.GetResponseStream(), true);
        }
    }
}
