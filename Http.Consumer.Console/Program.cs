namespace Http.Consumer.Console
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    class Program
    {
        static void Main(string[] args)
        {
            //Delegate 
            //https://github.com/microsoft/referencesource/blob/aaca53b025f41ab638466b1efe569df314f689ea/System/net/System/Net/Http/HttpClientHandler.cs
            //new HttpResponseMessage()
            //DelegatingHandler
            //new HttpRequestMessage()
            //new HttpContent()

            IHttpConsumer httpConsumer = new HttpConsumer();
            httpConsumer
                .Host("http://localhost:1977/api/")
                .Resource("values/d/", x => x.SetContentType(ContentType.Json)
                                            .AddResponseHeader(HttpResponseHeader.Allow,""))
                .Get<List<AgUser>>()
                .BuildAsync()
                .GetAwaiter()
                .GetResult();

            Console.WriteLine($"Hello World!");
        }

        private static void ServerPoint(LocalIPEndPoint obj)
        {
            obj.SetIPEndPoint(IPAddress.Parse("111.111.111.111"), 0);
        }
    }
}
