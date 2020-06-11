﻿using Newtonsoft.Json;
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

        [JsonIgnore]
        public string Fecha { get; set; }

        [JsonIgnore]
        private PublicacionModel publicacion;

        #endregion Properties

        #region Initialize
        public ComentarioModel(UsuarioModel usuario, PublicacionModel publicacion)
        {
            this.creador = usuario;
            this.idUsuario = usuario.idUsuario;
            this.Publicacion = publicacion;
            this.idPublicacion = publicacion.idPublicacion;
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
