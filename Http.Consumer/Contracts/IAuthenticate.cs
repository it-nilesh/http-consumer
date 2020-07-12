using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Http.Consumer.Contracts
{
    public interface IAuthenticate
    {
        IHttpConsumer NetworkCredential(ICredentials networkCredential, bool preAuthenticate = false);

        IHttpConsumer BasicAuthenticate(string userName, string password);

        IHttpConsumer BearerAuthenticate(string token);

        IHttpConsumer AddX509Certificate(Action<X509CertificateCollection> x509Certificates);

      //  Dictionary<string, string> Auth { get; }
    }
}
