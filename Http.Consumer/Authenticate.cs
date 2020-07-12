using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Http.Consumer
{
    using Http.Consumer.Contracts;
    using System.Text;

    public abstract class Authenticate : IAuthenticate
    {

        internal Dictionary<string, string> Auth { get; } = new Dictionary<string, string>();
        internal bool PreAuthenticate { get; private set; }
        internal ICredentials Credentials { get; private set; }

        public IHttpConsumer BasicAuthenticate(string userName, string password)
        {
            Auth.Clear();

            var token = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(userName + ":" + password));
            Auth[HttpRequestHeader.Authorization.ToString()] = $"Basic {token}";

            return (IHttpConsumer)this;
        }

        public IHttpConsumer NetworkCredential(ICredentials credentials, bool preAuthenticate = false)
        {
            Credentials = credentials;
            PreAuthenticate = preAuthenticate;

            return (IHttpConsumer)this;
        }
     
        public IHttpConsumer BearerAuthenticate(string token)
        {
            Auth.Clear();

            if (!token.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
            {
                token = $"bearer {token}";
            }

            Auth[HttpRequestHeader.Authorization.ToString()] = token;

            return (IHttpConsumer)this;
        }
      
        public X509CertificateCollection CertificateCollection { get; protected set; }
       
        public IHttpConsumer AddX509Certificate(Action<X509CertificateCollection> x509Certificates)
        {
            if (x509Certificates != null)
            {
                CertificateCollection = new X509CertificateCollection();
                x509Certificates(CertificateCollection);
            }

            return (IHttpConsumer)this;
        }
    }
}
