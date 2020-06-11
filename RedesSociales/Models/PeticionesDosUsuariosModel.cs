using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RedesSociales.Models
{
    public class PeticionesDosUsuariosModel : BaseModel
    {
        [JsonProperty("idUsuario1")]
        public int idUsuario1 { get; set; }
        [JsonProperty("idUsuario2")]
        public int idUsuario2 { get; set; }
    }
}
