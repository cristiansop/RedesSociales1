using RedesSociales.Models.Auxiliary;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RedesSociales.Servicios.Rest
{
    public abstract class Request<T>
    {
        #region Properties
        protected string Url { get; set; }
        protected string Verb { get; set; }
        protected string UrlParameters { get; set; }
        private static HeadersService headersService;
        #endregion Properties

        #region Gets / Sets
        protected static HeadersService HeadersService
        {
            get
            {
                if(headersService == null)
                {
                    headersService = new HeadersService();
                }
                return headersService;
            }
        }
        #endregion Gets / Sets

        #region Methods
        public abstract Task<APIResponse> SendRequest(T obj);

        public async Task BuildURL(ParametersRequest parameters)
        {
            string newURL = Url;
            if (parameters != null)
            {
                ParametersRequest Parameters = parameters as ParametersRequest;

                if (Parameters.Parameters.Count > 0)
                {
                    newURL = (newURL.Substring(Url.Length - 1) == "/") ? newURL.Remove(newURL.Length - 1) : newURL;
                    Parameters.Parameters.ForEach(p => newURL += "/" + p);
                }

                if (Parameters.QueryParameters.Count > 0)
                {
                    var queryParameters = await new FormUrlEncodedContent(Parameters.QueryParameters).ReadAsStringAsync();
                    newURL += queryParameters;
                }
            }
            UrlParameters = newURL;
        }

        #endregion Methods
    }
}
