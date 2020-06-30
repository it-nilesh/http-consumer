using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace Http.Consumer.RequestContent
{
    public class HttpMultipartFormDataContent : MultipartFormDataContent, IHttpRequestContent
    {
        public HttpMultipartFormDataContent(object formData) : base(Guid.NewGuid().ToString())
        {
            BindString(formData);
        }

        public void Add(string name, byte[] content, string fileName, MediaTypeHeaderValue mediaTypeHeader)
        {
            HttpContent bytesContent = new ByteArrayContent(content);
            bytesContent.Headers.ContentType = mediaTypeHeader;
            this.Add(bytesContent, name, fileName);
        }

        private void BindString(object formData)
        {
            if (formData == null)
                return;

            IDictionary formObjects = GetFormDictionary(formData);
            foreach (string key in formObjects.Keys)
            {
                var formValue = formObjects[key];
                var formType = formObjects[key]?.GetType()?.ToString();
                TryAddValue(key, formValue, formType);
            }
        }

        private void TryAddValue(string key, object formValue, string formType)
        {
            try
            {
                var value = Convert.ToString(formValue);
                if (value.Equals(formType))
                {
                    var values = formValue as IEnumerable;
                    foreach (var innerValue in values)
                    {
                        AddValue(key, innerValue);
                    }
                }
                else
                {
                    AddValue(key, value);
                }
            }
            catch { }
        }

        private void AddValue(string key, object val)
        {
            this.Add(new StringContent(val.ToString()), key.ToString());
        }

        private IDictionary GetFormDictionary(object formData)
        {
            IDictionary formObjects;
            if (formData is IDictionary dicForm)
                formObjects = dicForm;
            else
                formObjects = formData.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                   .ToDictionary(prop => prop.Name, prop => prop.GetValue(formData, null));
            return formObjects;
        }

        public void AddFiles(List<HttpRequestFileContent> files)
        {
            foreach (var payloadContent in files)
                this.Add(payloadContent.KeyName, payloadContent.File, payloadContent.FileName, new MediaTypeHeaderValue(payloadContent.FileMimeType));
        }
    }
}
