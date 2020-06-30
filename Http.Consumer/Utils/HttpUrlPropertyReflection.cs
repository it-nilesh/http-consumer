using System;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;

namespace Http.Consumer
{
    internal static class HttpUrlPropertyReflection
    {
        private static Action<HttpWebRequest, Uri> SetRequestUri { get; }

        static HttpUrlPropertyReflection()
        {
            Type requestType = typeof(HttpWebRequest);
            FieldInfo field = requestType.GetField("_requestUri", BindingFlags.NonPublic | BindingFlags.Instance);

            ParameterExpression targetExp = Expression.Parameter(requestType, "target");
            ParameterExpression valueExp = Expression.Parameter(typeof(Uri), "value");

            // Expression.Property can be used here as well
            MemberExpression fieldExp = Expression.Field(targetExp, field);
            BinaryExpression assignExp = Expression.Assign(fieldExp, valueExp);

            SetRequestUri = Expression.Lambda<Action<HttpWebRequest, Uri>>
                (assignExp, targetExp, valueExp).Compile();
        }

        public static void CreateUri(this HttpWebRequest httpWebRequest, Uri requestUri, object url)
        {
            SetRequestUri(httpWebRequest, new Uri(requestUri, $"{url}"));
        }
    }
}
