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
        public int idUsuario { get; set; }

        [JsonProperty("Apodo")]
        public string Apodo { get; set; }

        [JsonProperty("idPublicacion")]
        public int idPublicacion { get; set; }


        [JsonProperty("Archivo")]
        public string Archivo { get; set; }


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
            this.idUsuario = usuario.idUsuario;
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
                idUsuario = value.idUsuario;
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
