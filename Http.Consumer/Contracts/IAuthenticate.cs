using System.Collections.Generic;

namespace Http.Consumer.Contracts
{
    public interface IAuthenticate
    {
        IHttpConsumer BasicAuthenticate(string userName, string password, string domain = null, bool preAuthenticate = false);
        IHttpConsumer BearerAuthenticate(string token);
        Dictionary<string, string> Auth { get; }
    }
}
