using Http.Consumer.RequestContent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Http.Consumer.ResponseContent
{
    public class HttpResponseDelegate : HttpContentDelegate
    {
        private readonly Dictionary<string, Func<Stream, IHttpResponseContent>> delegateHttpResponse = new Dictionary<string, Func<Stream, IHttpResponseContent>>();

        public HttpResponseDelegate(HttpWebRequest httpWebRequest) : base(httpWebRequest)
        {
            delegateHttpResponse[ContentType.Json] = (value) => { return new HttpJsonResponseContent(value); };
        }

        public override async Task<T> ExecuteAsync<T>()
        {
            //TODO: Check other content type 
            T responseContent;

            var httpReposne = await base.ExecuteAsync();
            if (string.IsNullOrWhiteSpace(httpReposne.ContentType) && httpReposne.ContentLength == 0)
            {
                var tcs = new TaskCompletionSource<T>();
                tcs.SetResult(default(T));
                responseContent = await tcs.Task;
            }
            else
            {
                IHttpResponseContent httpResponseContent = delegateHttpResponse[httpReposne.ContentType.Split(';')[0]](httpReposne.GetResponseStream());
                responseContent = await httpResponseContent.DeserializeAsync<T>();
            }

            return responseContent;
        }

        public override async Task<Stream> ReceiveFileAsync()
        {
            var httpReposne = await base.ExecuteAsync();
            Stream stream = httpReposne.GetResponseStream();
            httpReposne.Close();
            httpReposne.Dispose();
            return stream;
        }
    }
}
