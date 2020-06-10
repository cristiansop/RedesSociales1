﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Models
{
    public class PeticionesComentarios
    {
        [JsonProperty("idUsuario")]
        public int idUsuario { get; set; }
        [JsonProperty("idPublicacion")]
        public int Idpublicacion { get; set; }
        [JsonProperty("idComentario")]
        public int Idcomentario { get; set; }
    }
}
