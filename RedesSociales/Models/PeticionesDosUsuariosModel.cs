using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RedesSociales.Models
{
    public class PeticionesDosUsuariosModel
    {
        [JsonProperty("idUsuario1")]
        public int Idusuario1 { get; set; }
        [JsonProperty("idUsuario2")]
        public int Idusuario2 { get; set; }
    }
}
