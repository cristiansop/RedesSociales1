using RedesSociales.Servicios.Propagacion;
using System;
using System.Collections.Generic;
using System.Text;


namespace RedesSociales.Models
{
	public class UsuarioModel : NotificationObject
	{
        #region Properties
        public int idusuario { get; set; }

        public string apodo { get; set; }

        private List<UsuarioModel> seguidos;

        private List<UsuarioModel> seguidores;

        private string nombre;

        private string apellidos;

        private string fotoperfil;

        private string estado;

        private List<PublicacionModel> publicaciones;

        #endregion Properties

        #region Getters/Setters
        public List<PublicacionModel> Publicaciones
        {
            get { return publicaciones; }
            set { publicaciones = value; OnPropertyChanged(); }
        }

        public string Estado
        {
            get { return estado; }
            set { estado = value; OnPropertyChanged(); }
        }

        public string FotoPerfil
        {
            get { return fotoperfil; }
            set { fotoperfil = value; OnPropertyChanged(); }
        }

        public string Apellidos
        {
            get { return apellidos; }
            set { apellidos = value; OnPropertyChanged(); }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; OnPropertyChanged(); }
        }

        public List<UsuarioModel> Seguidos
        {
            get { return seguidos; }
            set { seguidos = value; OnPropertyChanged(); }
        }

        public List<UsuarioModel> Seguidores
        {
            get { return seguidores; }
            set { seguidores = value; OnPropertyChanged(); }
        }

        #endregion Getters/Setters
    }
}
