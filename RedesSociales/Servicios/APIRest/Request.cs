using RedesSociales.Models.AuxiliarModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RedesSociales.Servicios.APIRest
{
    public abstract class Request<T>
    {
        protected string Url { get; set; }
        protected string Verbo { get; set; }

        protected string UrlParameters { get; set; }

        private static ServicioHeaders servicioHeaders;

        #region Getters/Setters
        protected static ServicioHeaders ServicioHeaders
        {
            get
            {
                if (servicioHeaders == null)
                {
                    servicioHeaders = new ServicioHeaders();
                }
                return servicioHeaders;

            }
        }
        #endregion Getters/Setters
        #region Metodos
        public abstract Task<APIResponse> SendRequest(T objecto);

        public async Task ConstruirURL(ParametersRequest parametros)
        {
            ParametersRequest Parametros = parametros as ParametersRequest;
            string newUrl=Url;
            if (Parametros.Parametros.Count > 0)
            {
                newUrl = (newUrl.Substring(Url.Length - 1) == "/") ? Url.Remove(newUrl.Length - 1) : newUrl;
                Parametros.Parametros.ForEach(p => newUrl += "/" + p);
            }

            if (Parametros.QueryParametros.Count > 0)
            {
                var queryParameters = await new FormUrlEncodedContent(Parametros.QueryParametros).ReadAsStringAsync();
                newUrl += queryParameters;

            }
        }
        #endregion Metodos
    }
}
