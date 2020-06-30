using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Http.Consumer
{
    using Http.Consumer.Contracts;

    public abstract class Authenticate : IAuthenticate
    {
        protected internal IHttpConsumer HttpConsumer { get; internal set; }

        internal ICredentials Credentials { get; private set; }

        internal bool PreAuthenticate { get; private set; }

        public IHttpConsumer BasicAuthenticate(string userName, string password, string domain = null, bool preAuthenticate = false)
        {
            if (string.IsNullOrWhiteSpace(domain))
                Credentials = new NetworkCredential(userName, password);
            else
                Credentials = new NetworkCredential(userName, password, domain);

            PreAuthenticate = preAuthenticate;

            return HttpConsumer;
        }

        public Dictionary<string, string> Auth { get; } = new Dictionary<string, string>();

        public IHttpConsumer BearerAuthenticate(string token)
        {
            Auth.Clear();

            if (!token.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
            {
                token = $"bearer {token}";
            }

            Auth.Add(HttpRequestHeader.Authorization.ToString(), token);
            return HttpConsumer;
        }


        public X509CertificateCollection CertificateCollection { get; protected set; }
        public IHttpConsumer AddX509Certificate(Action<X509CertificateCollection> x509Certificates)
        {
            if (x509Certificates != null)
            {
                CertificateCollection = new X509CertificateCollection();
                x509Certificates(CertificateCollection);
            }

            return HttpConsumer;
        }
    }
}
