using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Http.Consumer
{
    public class HeaderDictionary : Dictionary<string, IList<string>>
    {
        public HeaderDictionary(WebHeaderCollection headers)
        {
            for (int headerIndex = 0; headerIndex < headers.Count; headerIndex++)
                this.Add(headers.GetKey(headerIndex), headers.GetValues(headerIndex).ToList());
        }
    }
}
