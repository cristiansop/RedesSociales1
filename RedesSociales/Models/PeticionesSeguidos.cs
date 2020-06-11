using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Models
{
    public class PeticionesSeguidos : BaseModel
    {
        [JsonProperty("idUsuario")]
        public int idUsuario { get; set; }
        [JsonProperty("Apodo")]
        public string Apodo { get; set; }
    }
}
