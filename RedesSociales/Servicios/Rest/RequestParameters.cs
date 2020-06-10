using RedesSociales.Models.Auxiliary;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RedesSociales.Servicios.Rest
{
    public class RequestParameters<T> : Request<T>
    {
        public RequestParameters(string url, string verb) 
        {
            Url = url;
            Verb = verb;
        }
        #region Methods
        public override async Task<APIResponse> SendRequest(T obj)
        {
            APIResponse response = new APIResponse()
            {
                Code = 400,
                IsSuccess = false,
                Response = ""
            };
            try
            {
                using (var client = new HttpClient())
                {
                    HttpMethod verbHttp = (Verb == "GET") ? HttpMethod.Get : HttpMethod.Delete;
                    await BuildURL(obj);
                    HttpRequestMessage requestMessage = new HttpRequestMessage(verbHttp, UrlParameters);
                    requestMessage = HeadersService.AddHeaders(requestMessage);
                    HttpResponseMessage httpResponse = client.SendAsync(requestMessage).Result;
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
        private async Task BuildURL(T parameters)
        {
            
            if(parameters != null) 
            {
                ParametersRequest Parameters = parameters as ParametersRequest;
                if (Parameters.Parameters.Count > 0)
                {
                    Url = (Url.Substring(Url.Length - 1) == "/") ? Url.Remove(Url.Length - 1) : Url;
                    Parameters.Parameters.ForEach(p => Url += "/" + p);
                }

                if (Parameters.QueryParameters.Count > 0)
                {
                    var queryParameters = await new FormUrlEncodedContent(Parameters.QueryParameters).ReadAsStringAsync();
                    Url += queryParameters;
                }
            }   
        }
        #endregion Methods
    }
}
