using Newtonsoft.Json;
using RedesSociales.Servicios.Propagacion;
using System;
using System.Collections.Generic;
using System.Text;


namespace RedesSociales.Models
{
    public class UsuarioModel : BaseModel
    {
        #region Properties
        [JsonProperty("idUsuario")]
        public int idUsuario { get; set; }

        [JsonProperty("Apodo")]
        public string Apodo { get; set; }

        [JsonIgnore]
        private List<PeticionesSeguidos> seguidos;

        [JsonIgnore]
        private List<PeticionesSeguidos> seguidores;

        [JsonProperty("Nombre")]
        private string Nombre;

        [JsonProperty("Apellido")]
        private string Apellido;

        [JsonProperty("FotoPerfil")]
        private string FotoPerfil;

        [JsonProperty("Estado")]
        private string Estado;

        [JsonIgnore]
        private List<PublicacionModel> publicaciones;

        #endregion Properties

        #region Getters/Setters
        public UsuarioModel()
        {
            Publicaciones = new List<PublicacionModel>();
            Seguidores = new List<PeticionesSeguidos>();
            Seguidos = new List<PeticionesSeguidos>();
        }
        public List<PublicacionModel> Publicaciones
        {
            get { return publicaciones; }
            set { publicaciones = value; OnPropertyChanged(); }
        }

        public string EstadoP
        {
            get { return Estado; }
            set { Estado = value; OnPropertyChanged(); }
        }

        public string FotoPerfilP
        {
            get { return FotoPerfil; }
            set { FotoPerfil = value; OnPropertyChanged(); }
        }

        public string ApellidoP
        {
            get { return Apellido; }
            set { Apellido = value; OnPropertyChanged(); }
        }

        public string NombreP
        {
            get { return Nombre; }
            set { Nombre = value; OnPropertyChanged(); }
        }

        public List<PeticionesSeguidos> Seguidos
        {
            get { return seguidos; }
            set { seguidos = value; OnPropertyChanged(); }
        }

        public List<PeticionesSeguidos> Seguidores
        {
            get { return seguidores; }
            set { seguidores = value; OnPropertyChanged(); }
        }

        #endregion Getters/Setters
    }
}