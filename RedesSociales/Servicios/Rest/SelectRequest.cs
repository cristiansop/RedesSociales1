
using RedesSociales.Configuration;
using RedesSociales.Models.Auxiliary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedesSociales.Servicios.Rest
{
    public class SelectRequest<T>
    {
        #region Properties
        public Request<T> SendStrategy { get; set; }
        public ConfigurationRest ConfigurationRest { get; set; }
        #endregion Properties

        #region Initialize
        public SelectRequest() 
        {
            ConfigurationRest = new ConfigurationRest();
        }
        #endregion Initialize

        #region Methods
        public void SelectStrategy(string verb, string url)
        {
            var dictionary = ConfigurationRest.ConfigurationVerbs;
            string className;
            if (dictionary.TryGetValue(verb.ToUpper(), out className))
            {
                Type classType = Type.GetType(className);
                Type[] typeArgs = { typeof(T) };
                var genericClass = classType.MakeGenericType(typeArgs);
                SendStrategy = (Request<T>)Activator.CreateInstance(genericClass, url, verb.ToUpper());
            }
        }
        public async Task<APIResponse> RunStrategy (T obj, ParametersRequest parametersRequest = null)
        {
            await SendStrategy.BuildURL(parametersRequest);
            var response = await SendStrategy.SendRequest(obj);
            return response;
        }
        #endregion Methods
    }
}
