using System;
using System.Net;

namespace Http.Consumer
{
    public static class HttpBindIPEndPointExtensions
    {
        public static HttpWebRequest BindIPEndPoint(this HttpWebRequest httpWebRequest, Action<LocalIPEndPoint> IpEndPoint)
        {
            if (IpEndPoint != null)
            {
                ServicePoint servicePoint = httpWebRequest.ServicePoint;  //ServicePointManager.FindServicePoint(httpWebRequest.RequestUri);
                servicePoint.BindIPEndPointDelegate = (sPoint, remoteEndPoint, retryCount) =>
                {
                    var endPoint = new LocalIPEndPoint();
                    IpEndPoint(endPoint);
                    return endPoint.EndPoint;
                };
                servicePoint.ConnectionLeaseTimeout = 0;
            }
            httpWebRequest.KeepAlive= false;
            return httpWebRequest;
        }
    }
}
