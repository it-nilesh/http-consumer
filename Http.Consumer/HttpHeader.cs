using System.Net;
using System;

namespace Http.Consumer
{
    public class HttpHeader
    {
        private readonly HttpWebRequest _httpWebRequest;
        private readonly Authenticate _authenticate;
        public HttpHeader(HttpWebRequest httpWebRequest, Authenticate authenticate)
        {
            _httpWebRequest = httpWebRequest;
            _authenticate = authenticate;

            DefaultHeaderConfig();
        }

        public HttpHeader SetHttpVersion(Utils.HttpVersion httpVersion = Utils.HttpVersion.Version11)
        {
            _httpWebRequest.ProtocolVersion = GetVersion(httpVersion);
            return this;
        }

        public HttpHeader SetContentType(string contentType= ContentType.Json)
        {
            _httpWebRequest.ContentType = contentType;
            return this;
        }

        public HttpHeader AddHeader(string key, string value)
        {
            _httpWebRequest.Headers.Add(key, value);
            return this;
        }

        public HttpHeader AddRequestHeader(HttpRequestHeader requestHeader, string value)
        {
            _httpWebRequest.Headers.Add(requestHeader, value);
            return this;
        }

        public HttpHeader AddResponseHeader(HttpResponseHeader responseHeader, string value)
        {
            _httpWebRequest.Headers.Add(responseHeader, value);
            return this;
        }

        private Version GetVersion(Utils.HttpVersion httpVersion)
        {
            return httpVersion == Utils.HttpVersion.Version10 ? HttpVersion.Version10 :
                   httpVersion == Utils.HttpVersion.Version11 ? HttpVersion.Version11 :
                                                                 HttpVersion.Version11;
        }

        private void DefaultHeaderConfig()
        {
            _httpWebRequest.Credentials = _authenticate.Credentials;
            _httpWebRequest.PreAuthenticate = _authenticate.PreAuthenticate;

            if (_authenticate.Auth.TryGetValue(HttpRequestHeader.Authorization.ToString(), out string token))
                AddRequestHeader(HttpRequestHeader.Authorization, token);

            if (_authenticate.CertificateCollection != null)
                _httpWebRequest.ClientCertificates = _authenticate.CertificateCollection;

            SetContentType();
            SetHttpVersion();
        }
    }
}
