using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Configuracion
{
    public class ConfiguracionRest
    {
        #region Properties
        private readonly string NameSpaceRest;
        public Dictionary<string, string> VerbosConfiguracion {get;set;}
        #endregion Properties
        #region Initialize
        public ConfiguracionRest()
        {
            NameSpaceRest = "RedesSociales.Servicios.APIRest.";
            InicializarVerbosConfiguracion();

        }
        #endregion Initialize
        #region Metodos
        private void InicializarVerbosConfiguracion() 
        {
            VerbosConfiguracion = new Dictionary<string, string>();
            VerbosConfiguracion.Add("GET", string.Concat(NameSpaceRest, "RequestParametros`1"));
            VerbosConfiguracion.Add("POST", string.Concat(NameSpaceRest, "RequestBody`1"));
            VerbosConfiguracion.Add("PUT", string.Concat(NameSpaceRest, "RequestBody`1"));
            VerbosConfiguracion.Add("DELETE", string.Concat(NameSpaceRest, "RequestParametros`1"));

        }
        #endregion Metodos
    }
}
