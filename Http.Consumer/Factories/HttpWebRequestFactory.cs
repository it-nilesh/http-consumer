using System;
using System.Net;

namespace Http.Consumer.Factories
{
    internal class HttpWebRequestFactory
    {
        public static HttpWebRequest Create(Uri uri)
        {
            return WebRequest.CreateHttp(uri);
        }
    }
}
