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
        public RequestParametros(string url,string verbo) 
        {
            Url = url;
            Verbo = verbo;
        }
        #region Metodos
        public override async Task<APIResponse> SendRequest(T objecto)
        {
            APIResponse respuesta = new APIResponse()
            {
                Code = 400,
                isSuccess = false,
                Response = ""
            };
            try
            {
                using (var client = new HttpClient())
                {
                    var verboHttp = (Verbo == "GET") ? HttpMethod.Get : HttpMethod.Delete;
                    client.Timeout = TimeSpan.FromSeconds(500);
                    //await this.ConstruirURL(objecto);
                    HttpRequestMessage requestMessage = new HttpRequestMessage(verboHttp, UrlParameters);
                    requestMessage = ServicioHeaders.AgregarCabecera(requestMessage);
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
        private async Task ConstruirURL(T parametros)
        {
            ParametersRequest Parametros = parametros as ParametersRequest;
            if(Parametros.Parametros.Count > 0)
            {
                Url = (Url.Substring(Url.Length - 1) == "/") ? Url.Remove(Url.Length - 1) : Url;
                Parametros.Parametros.ForEach(p => Url += "/" + p);
            }
            if(Parametros.QueryParametros.Count >0)
            {
                var queryParameters = await new FormUrlEncodedContent(Parametros.QueryParametros).ReadAsStringAsync();
                Url += queryParameters;

            }
        }
        #endregion Metodos
    }
}
