using Newtonsoft.Json;
using RedesSociales.Servicios.Propagacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Models
{
    public class PublicacionModel: NotificationObject
    {
        #region Properties
        [JsonIgnore]
        private UsuarioModel creador;
        [JsonProperty("idUsuario")]
        public int Idusuario { get; set; }

        [JsonProperty("idPublicacion")]
        public int IdPublicacion { get; set; }
        [JsonProperty("Archivo")]
        public string Imagen { get; set; }
        [JsonProperty("Tipo")]
        public string Tipo { get; set; }
        [JsonProperty("Descripcion")]
        public string Descripcion { get; set; }

        [JsonIgnore]
        public string Fecha { get; set; }
        [JsonIgnore]
        private List<UsuarioModel> reacciones;
        [JsonIgnore]
        private List<UsuarioModel> etiquetas;
        [JsonIgnore]
        private List<ComentarioModel> comentarios;
        #endregion Properties

        #region Inicialize
        public PublicacionModel(UsuarioModel usuario)
        {
            this.creador = usuario;
            this.Idusuario = usuario.Idusuario;
        }

        public PublicacionModel()
        {
        }
        #endregion Inicialize

        #region Getters/Setters
        public UsuarioModel Creador
        {
            get { return creador; }
            set { creador = value;
                Idusuario = value.Idusuario;
                OnPropertyChanged(); }
        }

        public List<UsuarioModel> Etiquetas
        {
            get { return etiquetas; }
            set { etiquetas = value; OnPropertyChanged(); }
        }
        public List<UsuarioModel> Reacciones
        {
            get { return reacciones; }
            set { reacciones = value; OnPropertyChanged(); }
        }
        public List<ComentarioModel> Comentarios
        {
            get { return comentarios; }
            set { comentarios = value; OnPropertyChanged(); }
        }
        #endregion Getters/Setters
    }
}
