using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RedesSociales.Servicios.Rest
{
    public class HeadersService
    {
        #region Properties
        public Dictionary<string, string> Headers { get; set; }
        #endregion Properties

        #region Initialize
        public HeadersService()
        {
            Headers = new Dictionary<string, string>();
            Headers.Add("ContentType", "application/json");
        }
        #endregion Initialize

        #region Methods
        public HttpRequestMessage AddHeaders(HttpRequestMessage requestMessage)
        {
            foreach(var h in Headers)
            {
                requestMessage.Headers.Add(h.Key, h.Value);
            }
            return requestMessage;
        }
        #endregion Methods
    }
}
