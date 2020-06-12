using Newtonsoft.Json;
using RedesSociales.Servicios.Propagacion;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RedesSociales.Models
{
    public class PublicacionUsuarioModel : BaseModel
    {
        #region Properties

        [JsonProperty("idPublicacion")]
        public int idPublicacion { get; set; }


        [JsonProperty("Archivo")]
        public string Archivo { get; set; }


        [JsonProperty("Tipo")]
        public int Tipo { get; set; }


        [JsonProperty("Descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("Tiempo")]
        public string Tiempo { get; set; }

        [JsonIgnore]
        private ImageSource imagen;


        [JsonIgnore]
        private List<PeticionesSeguidos> reacciones;


        [JsonIgnore]
        private List<PeticionesSeguidos> etiquetas;


        [JsonIgnore]
        private List<PeticionesComentariosPublicacion> comentarios;
        #endregion Properties

        #region Inicialize

        public PublicacionUsuarioModel()
        {
            Comentarios = new List<PeticionesComentariosPublicacion>();
            Etiquetas = new List<PeticionesSeguidos>();
            Reacciones = new List<PeticionesSeguidos>();
        }
        #endregion Inicialize

        #region Getters/Setters

        public ImageSource Imagen
        {
            get { return imagen; }
            set
            {
                imagen = value;
                OnPropertyChanged();
            }
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
