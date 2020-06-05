using RedesSociales.Servicios.Propagacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Models
{
    public class PublicacionModel: NotificationObject
    {
        #region Properties
        private UsuarioModel creador;

        public int idPublicacion { get; set; }

        public string Imagen { get; set; }

        public string Fecha { get; set; }

        private List<UsuarioModel> reacciones; 

        private List<UsuarioModel> etiquetas;

        private List<ComentarioModel> comentarios;
        #endregion Properties
        #region Inicialize
        public PublicacionModel(UsuarioModel usuario)
        {
            this.creador = usuario;
        }
        #endregion Inicialize
        #region Getters/Setters
        public UsuarioModel Creador
        {
            get { return creador; }
            set { creador = value; OnPropertyChanged(); }
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
