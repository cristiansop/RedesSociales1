using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Models
{
    public class PeticionesComentariosPublicacion : BaseModel
    {
        [JsonProperty("idUsuario")]
        public int idUsuario { get; set; }

        [JsonProperty("Apodo")]
        public string Apodo { get; set; }

        [JsonProperty("idComentario")]
        public int idComentario { get; set; }

        [JsonProperty("Cuerpo")]
        public string Cuerpo { get; set; }

        [JsonProperty("Tiempo")]
        public string Tiempo { get; set; }
    }
}
