using Http.Consumer.Contracts;
using System.Net;

namespace Http.Consumer
{
    public class HttpResponse : IHttpResponse
    {
        public HeaderDictionary RequestHeader { get; }

        public HeaderDictionary ResponseHeader { get; }

        public HttpStatusCode StatusCode { get; }

        public HttpResponse(HttpStatusCode statusCode, HeaderDictionary requestHeader, HeaderDictionary responseHeader)
        {
            RequestHeader = requestHeader;
            ResponseHeader = responseHeader;
            StatusCode = statusCode;
        }
    }

    public class HttpResponse<T> : HttpResponse, IHttpResponse<T>
    {
        public HttpResponse(T content, HttpStatusCode statusCode, HeaderDictionary requestHeader, HeaderDictionary responseHeader) : base(statusCode, requestHeader, responseHeader)
        {
            Content = content;
        }

        public T Content { get; }
    }
}
