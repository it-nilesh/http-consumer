using Http.Consumer.Contracts;
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

        public HttpResponseDelegate(HttpWebRequest httpWebRequest, IReadOnlyCollection<IDeserializer> deserializers) : base(httpWebRequest)
        {
            delegateHttpResponse[ContentType.Json] = (value) => { return new HttpJsonResponseContent(value); };

            foreach (var deserializer in deserializers)
                delegateHttpResponse[deserializer.ContentType] = (value) => { return new HttpCustomDeserializer(deserializer, value); };
        }

        public override async Task<IHttpResponse<TResult>> ExecuteAsync<TResult>()
        {
            //TODO: Check other content type 
            TResult responseContent;

            var httpReposne = await base.ExecuteAsync();
            if (string.IsNullOrWhiteSpace(httpReposne.ContentType) && httpReposne.ContentLength == 0)
            {
                var tcs = new TaskCompletionSource<TResult>();
                tcs.SetResult(default(TResult));
                responseContent = await tcs.Task;
            }
            else
            {
                IHttpResponseContent httpResponseContent = delegateHttpResponse[httpReposne.ContentType.Split(';')[0]](httpReposne.GetResponseStream());
                responseContent = await httpResponseContent.DeserializeAsync<TResult>();
                httpResponseContent.Dispose();
            }

            return GetHttpReponse<TResult>(httpReposne, responseContent);
        }

        public override async Task<IHttpResponse<Stream>> ReceiveFileAsync()
        {
            var httpReposne = await base.ExecuteAsync();
            Stream stream = httpReposne.GetResponseStream();
            httpReposne.Close();
            httpReposne.Dispose();

            return GetHttpReponse(httpReposne, stream);
        }

        private IHttpResponse<T> GetHttpReponse<T>(HttpWebResponse httpReposne, T content)
        {
            return new HttpResponse<T>(content, httpReposne.StatusCode, new HeaderDictionary(HttpWebRequest.Headers), new HeaderDictionary(httpReposne.Headers));
        }
    }
}
