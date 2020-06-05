using Newtonsoft.Json;
using RedesSociales.Models.AuxiliarModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RedesSociales.Servicios.APIRest
{
    public class RequestBody<T> : Request<T>
    {
        public RequestBody(string url, string verbo)
        {
            Url = url;
            Verbo = verbo;
        }

        public override async Task<APIResponse> SendRequest(T objecto)
        {
            APIResponse respuesta = new APIResponse()
            {
                Code = 400,
                isSuccess = false,
                Response = ""
            };
            string objetoJson = JsonConvert.SerializeObject(objecto);
            HttpContent content = new StringContent(objetoJson, Encoding.UTF8);

            try
            {
                using (var client = new HttpClient())
                {
                    var verboHttp = (Verbo == "POST") ? HttpMethod.Post : HttpMethod.Put;
                    HttpRequestMessage requestMessage = new HttpRequestMessage(verboHttp, Url);
                    requestMessage = ServicioHeaders.AgregarCabecera(requestMessage);
                    requestMessage.Content = content;
                    HttpResponseMessage HttpResponse = await client.SendAsync(requestMessage);
                    respuesta.Code = Convert.ToInt32(HttpResponse.StatusCode);
                    respuesta.isSuccess = HttpResponse.IsSuccessStatusCode;
                    respuesta.Response = await HttpResponse.Content.ReadAsStringAsync();

                }

            }
            catch (Exception)
            {

                respuesta.Response = "error  al momento de llamar servidor";
            }
            return respuesta;
        }
    }
}
