using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RedesSociales.Servicios.APIRest
{
    public class ServicioHeaders
    {
        #region Atributos
        public Dictionary<string, string> Headers { get; set; }
        #endregion Atributos

        #region Inicializador
        public ServicioHeaders()
        {
            Headers = new Dictionary<string, string>();
            Headers.Add("ContentType", "application/json");
        }
        #endregion Inicializador

        #region Métodos
        public HttpRequestMessage AgregarCabeceras(HttpRequestMessage requestMessage)
        {
            foreach (var h in Headers)
            {
                requestMessage.Headers.Add(h.Key, h.Value);
            }
            return requestMessage;
        }
        #endregion Métodos
    }
}
