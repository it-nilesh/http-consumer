namespace Http.Consumer.Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            IHttpConsumer httpConsumer = new HttpConsumer();
            var val = httpConsumer
                    .Host("http://localhost:1977/api/")                   
                    .AddDeserializer(new NewtoneJsonSerialize())
                    // .AddSerializer(new NewtoneJsonSerialize())
                    .Resource("BasicAuth/", x => x.SetContentType(ContentType.Json))
                    .Get<User>()
                    .BuildAsync()
                    .GetAwaiter()
                    .GetResult();

            //var val = httpConsumer
            //        .Host("http://localhost:1977/api/")
            //        .AddDeserializer(new NewtoneJsonSerialize())
            //        // .AddSerializer(new NewtoneJsonSerialize())
            //        .Resource("values/v/", x => x.SetContentType(ContentType.Json))
            //        .Get<User>()
            //        .Next("values/d/")
            //        .Get<User>()
            //        .Aggregate<AgUser>((x, y) =>
            //        {
            //            x.Get<User>(0, y)
            //             .Bind(x => x.Name1, z => z.Name1)
            //             .Get<User>(1, y)
            //             .Bind(x => x.Name1, z => z.Name);
            //        })
            //        .BuildAsync()
            //        .GetAwaiter()
            //        .GetResult();

            Console.WriteLine($"Hello World!");
        }
    }

    public class NewtoneJsonSerialize : ISerializer, IDeserializer
    {
        public string ContentType => "application/json";

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions();

        public NewtoneJsonSerialize()
        {
            _options.IgnoreNullValues = true;
            _options.IgnoreReadOnlyProperties = true;
            _options.PropertyNameCaseInsensitive = true;
        }

        public async Task<T> DeserializeAsync<T>(Stream stream)
        {
            return await JsonSerializer.DeserializeAsync<T>(stream, _options);
        }

        public void Dispose()
        {

        }

        public async Task<Stream> SerializeAsync(object value)
        {
            MemoryStream _stream = new MemoryStream();
            JsonSerializer.Serialize(new Utf8JsonWriter(_stream), value, value.GetType(), _options);
            return await Task.FromResult(_stream);
        }
    }
}
