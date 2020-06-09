using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Models.AuxiliarModels
{
    public class ParametersRequest
    {
        #region Properties
        public List<string> Parametros { get; set; }
        public Dictionary<string, string> QueryParametros { get; set; }
        #endregion Properties

        #region Initialize
        public ParametersRequest()
        {
            Parametros = new List<string>();
            QueryParametros = new Dictionary<string, string>();
        }
        #endregion Initialize

    }
}
