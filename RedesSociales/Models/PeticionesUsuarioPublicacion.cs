using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Models
{
    public class PeticionesUsuarioPublicacion
    {
        [JsonProperty("idUsuario")]
        public int idUsuario { get; set; }
        [JsonProperty("idPublicacion")]
        public int idPublicacion { get; set; }
    }
}
