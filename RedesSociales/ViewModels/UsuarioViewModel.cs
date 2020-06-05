using System;
using System.Collections.Generic;
using System.Text;
using RedesSociales.Configuracion;
using RedesSociales.Models;
using RedesSociales.Models.AuxiliarModels;
using RedesSociales.Servicios.APIRest;
using RedesSociales.Servicios.Propagacion;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
namespace RedesSociales.ViewModels
{
    public class UsuarioViewModel:NotificationObject
    {
        public ElegirRequest<UsuarioModel> CreateUsuario { get; set; }
        public ElegirRequest<BaseModel> GetUsuario { get; set; }
        public ElegirRequest<UsuarioModel> UpdateUsuario { get; set; }
        public ElegirRequest<BaseModel> DeleteUsuario { get; set; }
        public ElegirRequest<BaseModel> CreateSeguir{ get; set; }
        public ElegirRequest<BaseModel> GetSeguidos { get; set; }
        public ElegirRequest<BaseModel> GetSeguidores{ get; set; }
        public ElegirRequest<BaseModel> DeleteSeguir { get; set; }

        public UsuarioViewModel()
        {
            string urlCretateUsuario=Endpoints.URL_SERVIDOR+ Endpoints.CREATE_USUARIO;
            string urlGetUsuario = Endpoints.URL_SERVIDOR + Endpoints.GET_USUARIO;
            string urlUpdateUsuario = Endpoints.URL_SERVIDOR + Endpoints.UPDATE_USUARIO;
            string urlDeleteUsuario = Endpoints.URL_SERVIDOR + Endpoints.DELETE_USUARIO;
            string urlCreateSeguir = Endpoints.URL_SERVIDOR + Endpoints.CREATE_SEGUIR;
            string urlGetSeguidos = Endpoints.URL_SERVIDOR + Endpoints.GET_SEGUIDOS;
            string urlGetSeguidores = Endpoints.URL_SERVIDOR + Endpoints.GET_SEGUIDORES;
            string urlDeleteSeguir = Endpoints.URL_SERVIDOR + Endpoints.DELETE_SEGUIR;
        }

    }
}
