using RedesSociales.Models.AuxiliarModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Net.Http;
using System.Threading.Tasks;


namespace RedesSociales.Servicios.APIRest
{
    public class RequestParametros<T> : Request<T>
    {
        /*Verbos GET y DELETE  */
        public RequestParametros(string url, string verbo)
        {
            Url = url;
            Verbo = verbo;
        }

        #region Métodos
        public override async Task<APIResponse> SendRequest(T objecto)
        {
            APIResponse respuesta = new APIResponse()
            {
                Code = 400,
                IsSuccess = false,
                Response = ""
            };

            try
            {
                using (var client = new HttpClient())
                {
                    var verboHttp = (Verbo == "GET") ? HttpMethod.Get : HttpMethod.Delete;
                    client.Timeout = TimeSpan.FromSeconds(50);
                    HttpRequestMessage requestMessage = new HttpRequestMessage(verboHttp, UrlParameters);
                    requestMessage = ServicioHeaders.AgregarCabeceras(requestMessage);
                    HttpResponseMessage HttpResponse = client.SendAsync(requestMessage).Result;
                    respuesta.Code = Convert.ToInt32(HttpResponse.StatusCode);
                    respuesta.IsSuccess = HttpResponse.IsSuccessStatusCode;
                    respuesta.Response = await HttpResponse.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                respuesta.Response = "Error al momento de llamar al servidor";
            }

            return respuesta;
        }
        #endregion Métodos


    }
}
