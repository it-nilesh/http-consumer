using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Http.Consumer.RequestContent
{
    public class HttpRequestDelegate : HttpContentDelegate
    {
        private readonly Dictionary<string, Func<object, IHttpRequestContent>> delegateHttpRequest = new Dictionary<string, Func<object, IHttpRequestContent>>();

        public HttpRequestDelegate(HttpWebRequest httpWebRequest, IReadOnlyCollection<ISerializer> serializers) : base(httpWebRequest)
        {
            delegateHttpRequest[ContentType.Json] = (value) => { return new HttpJsonRequestContent(value); };
            delegateHttpRequest[ContentType.FormUrlencoded] = (value) => { return HttpFormUrlEncodedContent.GetHttpContent(value); };
            delegateHttpRequest[ContentType.MultipartFormData] = (value) => { return new HttpMultipartFormDataContent(value); };

            foreach (var serializer in serializers)
                delegateHttpRequest[serializer.ContentType] = (value) => { return new HttpCustomSerializer(serializer, value); };
        }

        public override async Task<HttpWebRequest> ExecuteAsync(HttpRequestContent<object> payload)
        {
            using (var streamWriter = await HttpWebRequest.GetRequestStreamAsync())
            {
                IHttpRequestContent httpContent = delegateHttpRequest[HttpWebRequest.ContentType](payload.Content);

                HttpWebRequest.ContentType = httpContent.Headers.ContentType.ToString();
                httpContent.AddFiles(payload.Files);
                await httpContent.CopyToAsync(streamWriter);
                httpContent.Dispose();
                streamWriter.Close();
            }

            return HttpWebRequest;
        }
    }
}
