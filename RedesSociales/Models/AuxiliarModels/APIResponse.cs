using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Models.AuxiliarModels
{
    public class APIResponse
    {
        #region Properties
        public int Code { get; set; }

        public string Response { get; set; }

        public bool isSuccess { get; set; }
        #endregion Properties
        #region Initialize
        public APIResponse() { }
        #endregion Initialize
    }
}
