using RedesSociales.Models.Auxiliary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RedesSociales.Servicios.Rest
{
    public class RequestBody<T> : Request<T>
    {
        public RequestBody(string url, string verb)
        {
            Url = url;
            Verb = verb;
        }
        public override async Task<APIResponse> SendRequest(T obj)
        {
            APIResponse response = new APIResponse()
            {
                Code = 400,
                IsSuccess = false,
                Response = ""
            };
            string objectJson = JsonConvert.SerializeObject(obj);
            HttpContent content = new StringContent(objectJson, Encoding.UTF8, "application/json");

            try
            {
                using (var client = new HttpClient())
                {
                    HttpMethod verbHttp = (Verb == "POST") ? HttpMethod.Post : HttpMethod.Put;
                    HttpRequestMessage requestMessage = new HttpRequestMessage(verbHttp, UrlParameters);
                    requestMessage = HeadersService.AddHeaders(requestMessage);
                    requestMessage.Content = content;
                    HttpResponseMessage httpResponse = await client.SendAsync(requestMessage);
                    response.Code = Convert.ToInt32(httpResponse.StatusCode);
                    response.IsSuccess = httpResponse.IsSuccessStatusCode;
                    response.Response = await httpResponse.Content.ReadAsStringAsync();

                }
            }
            catch (Exception)
            {
                response.Response = "Error al momento de llamar al servidor";
            }

            return response;
        }
    }
}
