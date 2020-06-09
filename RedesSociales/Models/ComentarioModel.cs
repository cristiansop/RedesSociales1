using Newtonsoft.Json;
using RedesSociales.Servicios.Propagacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Models
{
    public class ComentarioModel : NotificationObject
    {
        #region Properties
        [JsonIgnore]
        private UsuarioModel creador;
        [JsonProperty("idUsuario")]
        public int IdUsuario { get; set; }
        [JsonProperty("idComentario")]
        public int IdComentario { get; set; }
        [JsonProperty("Cuerpo")]
        public string Cuerpo { get; set; }
        [JsonProperty("idPublicacion")]
        public int Idpublicacion { get; set; }
        [JsonIgnore]
        public string Fecha { get; set; }
        [JsonIgnore]
        private PublicacionModel publicacion;

        #endregion Properties
        #region Initialize
        public ComentarioModel(UsuarioModel usuario, PublicacionModel publicacion)
        {
            this.creador = usuario;
            this.IdUsuario = usuario.Idusuario;
            this.Publicacion = publicacion;
            this.Idpublicacion = publicacion.IdPublicacion;
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
