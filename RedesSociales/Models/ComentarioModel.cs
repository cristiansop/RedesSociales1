using RedesSociales.Servicios.Propagacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Models
{
    public class ComentarioModel : NotificationObject
    {
        #region Properties
        private UsuarioModel creador;

        public int idComentario { get; set; }

        public string Cuerpo { get; set; }

        public string Fecha { get; set; }

        private PublicacionModel publicacion;

        #endregion Properties
        #region Initialize
        public ComentarioModel(UsuarioModel usuario, PublicacionModel publicacion)
        {
            this.creador = usuario;
            this.publicacion = publicacion;
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
