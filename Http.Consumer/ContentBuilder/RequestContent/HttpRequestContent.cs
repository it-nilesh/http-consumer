using System.Collections.Generic;
using System.Net.Http;

namespace Http.Consumer.RequestContent
{
    public class HttpRequestFileContent 
    {
        internal byte[] File { get; }
        internal string FileName { get; }
        internal string KeyName { get; }
        internal string FileMimeType { get; }

        public HttpRequestFileContent(string keyName, byte[] file, string fileName, string fileMimeType)
        {
            File = file;
            FileName = fileName;
            FileMimeType = fileMimeType;
            KeyName = keyName;
        }

        protected HttpRequestFileContent()
        {
        }
    }

    public class HttpRequestContent<T> : HttpRequestFileContent
    {
        internal T Content { get; }

        public List<HttpRequestFileContent> Files { get; private set; }

        public HttpRequestContent(T content)
        {
            Content = content;
        }

        public HttpRequestContent(T content, string keyName, byte[] file, string fileName, string fileMimeType)
        {
            Content = content;
            AddFile(keyName, file, fileName, fileMimeType);
        }

        public void AddFile(string keyName, byte[] file, string fileName, string fileMimeType = ContentType.OctetStream)
        {
            if (Files == null)
                Files = new List<HttpRequestFileContent>();

            Files.Add(new HttpRequestFileContent(keyName, file, fileName, fileMimeType));
        }
    }

    public class HttpRequestQueryString
    {
        private readonly List<KeyValuePair<string, string>> _queryString = new List<KeyValuePair<string, string>>();

        public HttpRequestQueryString AddParams(string key, string value)
        {
            _queryString.Add(new KeyValuePair<string, string>(key, value));

            return this;
        }

        internal string GetFullQueryString()
        {
            var content = new FormUrlEncodedContent(_queryString);
            string queryString = content.ReadAsStringAsync().ConfigureAwait(false)
                                              .GetAwaiter()
                                              .GetResult();
            _queryString.Clear();
            return queryString;
        }
    }
}
