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
            HttpContent content = new StringContent(objetoJson, Encoding.UTF8, "application/json");

            try
            {
                using (var client = new HttpClient())
                {
                    var verboHttp = (Verbo == "POST") ? HttpMethod.Post : HttpMethod.Put;
                    //HttpRequestMessage requestMessage = new HttpRequestMessage(verboHttp, Url);
                    HttpRequestMessage requestMessage = new HttpRequestMessage(verboHttp, UrlParameters);
                    //requestMessage = ServicioHeaders.AgregarCabecera(requestMessage);
                    requestMessage.Content = content;
                    client.Timeout = TimeSpan.FromSeconds(50);//changed
                    //HttpResponseMessage HttpResponse = await client.SendAsync(requestMessage);
                    HttpResponseMessage HttpResponse = client.SendAsync(requestMessage).Result;
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
