using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Http.Consumer.RequestContent
{
    public class HttpRequestDelegate : HttpContentDelegate
    {
        private readonly Dictionary<string, Func<object, IHttpRequestContent>> delegateHttpRequest = new Dictionary<string, Func<object, IHttpRequestContent>>();

        public HttpRequestDelegate(HttpWebRequest httpWebRequest) : base(httpWebRequest)
        {
            delegateHttpRequest[ContentType.Json] = (value) => { return new HttpJsonRequestContent(value); };
            delegateHttpRequest[ContentType.FormUrlencoded] = (value) => { return HttpFormUrlEncodedContent.GetHttpContent(value); };
            delegateHttpRequest[ContentType.MultipartFormData] = (value) => { return new HttpMultipartFormDataContent(value); };
        }

        public override async Task<HttpWebRequest> ExecuteAsync(HttpRequestContent<object> payload)
        {
            using (var streamWriter = await _httpWebRequest.GetRequestStreamAsync())
            {
                IHttpRequestContent httpContent = delegateHttpRequest[_httpWebRequest.ContentType](payload.Content);

                _httpWebRequest.ContentType = httpContent.Headers.ContentType.ToString();
                httpContent.AddFiles(payload.Files);
                await httpContent.CopyToAsync(streamWriter);
                httpContent.Dispose();
                streamWriter.Close();
            }

            return _httpWebRequest;
        }
    }
}
