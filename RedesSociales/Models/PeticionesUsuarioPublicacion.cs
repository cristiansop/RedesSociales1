using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Models
{
    public class PeticionesUsuarioPublicacion
    {
        [JsonProperty("idUsuario")]
        public int Idusuario { get; set; }
        [JsonProperty("idPublicacion")]
        public int Idpublicacion { get; set; }
    }
}
