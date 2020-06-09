using System;
using System.Collections.Generic;
using System.Text;

namespace RedesSociales.Configuracion
{
    class Endpoints
    {
        public static readonly string URL_SERVIDOR= "http://localhost:9000";
        #region Usuario
        public static readonly string GET_USUARIO = "/usuario/get/";
        public static readonly string CREATE_USUARIO = "/usuario/create";
        public static readonly string UPDATE_USUARIO = "/usuario/update";
        public static readonly string DELETE_USUARIO = "/usuario/delete";

        public static readonly string CREATE_SEGUIR = "/usuario/createSeguir";
        public static readonly string GET_SEGUIDOS = "/usuario/getSeguidos/";//$idUsuario<\d+>;
        public static readonly string GET_SEGUIDORES = "/usuario/getSeguidores/";//$idUsuario<\d+>;
        public static readonly string DELETE_SEGUIR = "/usuario/getSeguidores/";
        #endregion Usuario
        #region Publicacion
        public static readonly string CREATE_PUBLICACION = "/publicacion/create";
        public static readonly string GET_PUBLICACIONES_SEGUIDOS = "/publicacion/getseguidos/";//$idUsuario<\d+>;
        public static readonly string GET_PUBLICACIONES_USUARIO = "/publicacion/getusuario/";//$idUsuario<\d+>;
        public static readonly string DELETE_PUBLICACIONES = "/publicacion/delete";
        public static readonly string CREATE_LIKE = "/publicacion/createLike";
        public static readonly string GET_LIKES = "/publicacion/getLikes/";//$idPublicacion<\d+>;
        public static readonly string DELETE_LIKE = "/publicacion/deleteLike";
        public static readonly string CREATE_ETIQUETA = "/publicacion/createEtiqueta";
        public static readonly string GET_ETIQUETAS = "/publicacion/getEtiquetas/";//$idPublicacion<\d+>;
        public static readonly string DELETE_ETIQUETA = "/publicacion/deleteEtiqueta";
        #endregion Publicacion
        #region Comentario
        public static readonly string CREATE_COMENTARIO = "/comentario/create";
        public static readonly string GET_COMENTARIOS = "/comentario/get/";//$idPublicacion<\d+>;
        public static readonly string DELETE_COMENTARIO = "/comentario/delete";
        #endregion Comentario

    }
}
