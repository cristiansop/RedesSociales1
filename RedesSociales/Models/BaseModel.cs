using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RedesSociales.Servicios.Propagacion;

namespace RedesSociales.Models
{
    public class BaseModel: NotificationObject
    {
        #region Properties
        [JsonIgnore]
        public int ID { get; set; }
        #endregion Properties
    }
}
