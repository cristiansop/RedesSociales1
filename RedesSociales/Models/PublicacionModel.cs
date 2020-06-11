using Newtonsoft.Json;
using RedesSociales.Servicios.Propagacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Models
{
    public class PublicacionModel : BaseModel
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

        [JsonProperty("Tiempo")]
        public string Tiempo { get; set; }


        [JsonIgnore]
        private List<PeticionesSeguidos> reacciones;


        [JsonIgnore]
        private List<PeticionesSeguidos> etiquetas;


        [JsonIgnore]
        private List<PeticionesComentariosPublicacion> comentarios;
        #endregion Properties

        #region Inicialize
        public PublicacionModel(UsuarioModel usuario)
        {
            this.creador = usuario;
            this.idUsuario = usuario.idUsuario;
            this.Apodo = usuario.Apodo;
            Comentarios = new List<PeticionesComentariosPublicacion>();
            Etiquetas = new List<PeticionesSeguidos>();
            Reacciones = new List<PeticionesSeguidos>();
            Tiempo = "";
        }

        public PublicacionModel()
        {
            Comentarios = new List<PeticionesComentariosPublicacion>();
            Etiquetas = new List<PeticionesSeguidos>();
            Reacciones = new List<PeticionesSeguidos>();
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

        public List<PeticionesSeguidos> Etiquetas
        {
            get { return etiquetas; }
            set { etiquetas = value; OnPropertyChanged(); }
        }
        public List<PeticionesSeguidos> Reacciones
        {
            get { return reacciones; }
            set { reacciones = value; OnPropertyChanged(); }
        }
        public List<PeticionesComentariosPublicacion> Comentarios
        {
            get { return comentarios; }
            set { comentarios = value; OnPropertyChanged(); }
        }
        #endregion Getters/Setters
    }
}
