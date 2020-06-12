using Newtonsoft.Json;
using RedesSociales.Servicios.Propagacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Models
{
    public class ComentarioModel : BaseModel
    {
        #region Properties

        [JsonIgnore]
        private UsuarioModel creador;

        [JsonProperty("idUsuario")]
        public int idUsuario { get; set; }

        [JsonProperty("idComentario")]
        public int idComentario { get; set; }

        [JsonProperty("Cuerpo")]
        public string Cuerpo { get; set; }

        [JsonProperty("idPublicacion")]
        public int idPublicacion { get; set; }

        [JsonProperty("Tiempo")]
        public string Tiempo { get; set; }

        [JsonIgnore]
        private PublicacionModel publicacion;

        #endregion Properties

        #region Initialize
        public ComentarioModel(int usuarioT, int publicacionT, string CuerpoT)
        {
            idUsuario = usuarioT;
            idPublicacion = publicacionT;
            Cuerpo = CuerpoT;
            idComentario = 0;
            Tiempo = "";
        }
        #endregion Initialize

        #region Getters/Setters

        public UsuarioModel Creador
        {
            get { return creador; }
            set { creador = value; OnPropertyChanged(); }
        }

        public PublicacionModel Publicacion
        {
            get { return Publicacion; }
            set { Publicacion = value; OnPropertyChanged(); }
        }
        #endregion Getters/Setters
    }
}
