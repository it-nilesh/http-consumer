using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Http.Consumer.RequestContent
{
    public class HttpFormUrlEncodedContent : FormUrlEncodedContent, IHttpRequestContent
    {
        public HttpFormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection) : base(nameValueCollection)
        {
        }

        public static IHttpRequestContent GetHttpContent(object data)
        {
            Dictionary<string, string> request = new Dictionary<string, string>();

            if (data != null)
                foreach (var property in data.GetType().GetProperties())
                {
                    var value = Convert.ToString(property.GetValue(data));
                    if (!value.Equals(property.PropertyType.FullName))
                        request.Add(property.Name, value);
                }

            return new HttpFormUrlEncodedContent(request);
        }

        public void AddFiles(List<HttpRequestFileContent> files)
        {
        }
    }
}
