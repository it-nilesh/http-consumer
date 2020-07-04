using Http.Consumer.Contracts;
using Http.Consumer.RequestContent;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Http.Consumer
{
    public abstract class HttpContentDelegate : IDisposable
    {
        protected HttpWebRequest HttpWebRequest { get; }

        protected HttpContentDelegate(HttpWebRequest httpWebRequest)
        {
            HttpWebRequest = httpWebRequest;
        }

        public virtual Task<HttpWebRequest> ExecuteAsync(HttpRequestContent<object> payload)
        {
            throw new System.NotImplementedException();
        }

        public virtual async Task<HttpWebResponse> ExecuteAsync()
        {
            return (await HttpWebRequest.GetResponseAsync()) as HttpWebResponse;
        }

        public virtual Task<IHttpResponse<TResult>> ExecuteAsync<TResult>()
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<IHttpResponse<Stream>> ReceiveFileAsync()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        // To detect redundant calls
        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                HttpWebRequest.Headers.Clear();
                // Dispose managed state (managed objects).
                // _httpWebRequest?.Abort();
            }

            _disposed = true;
        }
    }
}
