using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Models
{
    public class PeticionIdApodo : BaseModel
    {
        [JsonProperty("idUsuario")]
        public int idUsuario { get; set; }
        [JsonProperty("Apodo")]
        public int Apodo { get; set; }
    }
}
