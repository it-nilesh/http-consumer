using Http.Consumer.RequestContent;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Http.Consumer
{
    public abstract class HttpContentDelegate: IDisposable 
    {
        protected readonly HttpWebRequest _httpWebRequest;

        protected HttpContentDelegate(HttpWebRequest httpWebRequest)
        {
            _httpWebRequest = httpWebRequest;
        }
        
        public virtual Task<HttpWebRequest> ExecuteAsync(HttpRequestContent<object> payload)
        {
            throw new System.NotImplementedException();
        }

        public virtual async Task<HttpWebResponse> ExecuteAsync()
        {
            return (await _httpWebRequest.GetResponseAsync()) as HttpWebResponse;
        }

        public virtual Task<TResult> ExecuteAsync<TResult>()
        {
            throw new System.NotImplementedException();
        }
     
        public virtual Task<Stream> ReceiveFileAsync()
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
                _httpWebRequest.Headers.Clear();
                // Dispose managed state (managed objects).
                // _httpWebRequest?.Abort();
            }

            _disposed = true;
        }
    }
}
