using RedesSociales.Configuracion;
using RedesSociales.Models;
using RedesSociales.Models.Auxiliary;
using RedesSociales.Servicios.Rest;
using RedesSociales.Servicios.Propagacion;
using RedesSociales.Validations.Base;
using RedesSociales.Validations.Rules;
using RedesSociales.Views;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using RedesSociales.Servicios.Handler;

namespace RedesSociales.ViewModels
{
    public class UsuarioViewModel: ViewModelBase
    {
        #region Properties

        #region Atributes
        private LoadDataHandler loadDataHandler;

        public MessagePopupView PopUp { get; set; }

        private UsuarioModel usuario;
        public UsuarioModel UsuarioMemoria { get; set; }

        #region Enables

        private bool isSeguirEnable;    

        #endregion Enables

        #endregion Atributes

        #region Request
        public SelectRequest<PeticionesDosUsuariosModel> CreateSeguir { get; set; }
        public SelectRequest<BaseModel> GetSeguidos { get; set; }
        public SelectRequest<BaseModel> GetSeguidores { get; set; }
        public SelectRequest<PeticionesDosUsuariosModel> DeleteSeguir { get; set; }
        public SelectRequest<BaseModel> GetPublicacionesUsuario { get; set; }
        #endregion Request

        #region Commands
        public ICommand CreateSeguirCommand { get; set; }
        public ICommand GetSeguidosCommand { get; set; }
        public ICommand GetSeguidoresCommand { get; set; }
        public ICommand DeleteSeguirCommand { get; set; }
        public ICommand GetPublicacionesUsuarioCommand { get; set; }

        #endregion Commands

        #endregion Properties

        #region Getters/Setters

        public UsuarioModel Usuario
        {
            get { return usuario; }
            set { usuario = value;
                //ActualizarPerfil();
                OnPropertyChanged(); }
        }
        public bool IsSeguirEnable
        {
            get { return isSeguirEnable; }
            set
            {
                isSeguirEnable = value;
                OnPropertyChanged();
            }
        }
        #endregion Getters/Setters

        #region Initialize
        public UsuarioViewModel()
        {
            PopUp = new MessagePopupView();
            Usuario = new UsuarioModel();
            UsuarioMemoria = (UsuarioModel)Application.Current.Properties["Usuario"];
            IsSeguirEnable =true;
            loadDataHandler = new LoadDataHandler();
            InitializeRequest();
            InitializeCommands();
            //ActualizarPerfil();

        }
        public void InitializeRequest()
        {
            #region Url
            string urlCreateSeguir = Endpoints.URL_SERVIDOR + Endpoints.CREATE_SEGUIR;
            string urlGetSeguidos = Endpoints.URL_SERVIDOR + Endpoints.GET_SEGUIDOS;
            string urlGetSeguidores = Endpoints.URL_SERVIDOR + Endpoints.GET_SEGUIDORES;
            string urlDeleteSeguir = Endpoints.URL_SERVIDOR + Endpoints.DELETE_SEGUIR;
            string urlGetPublicacionesUsuario = Endpoints.URL_SERVIDOR + Endpoints.GET_PUBLICACIONES_USUARIO;
            #endregion Url

            #region API

            CreateSeguir = new SelectRequest<PeticionesDosUsuariosModel>();
            CreateSeguir.SelectStrategy("POST", urlCreateSeguir);

            GetSeguidos = new SelectRequest<BaseModel>();
            GetSeguidos.SelectStrategy("GET", urlGetSeguidos);

            GetSeguidores = new SelectRequest<BaseModel>();
            GetSeguidores.SelectStrategy("GET", urlGetSeguidores);

            DeleteSeguir = new SelectRequest<PeticionesDosUsuariosModel>();
            DeleteSeguir.SelectStrategy("POST", urlDeleteSeguir);

            GetPublicacionesUsuario = new SelectRequest<BaseModel>();
            GetPublicacionesUsuario.SelectStrategy("GET", urlGetPublicacionesUsuario);

            #endregion API
        }
        public void InitializeCommands()
        {
            #region Comandos
            CreateSeguirCommand = new Command(async () => await CrearSeguir(), () => true);
            GetSeguidosCommand = new Command(async () => await SeleccionarSeguidos(), () => true);
            GetSeguidoresCommand = new Command(async () => await SeleccionarSeguidores(), () => true);
            DeleteSeguirCommand = new Command(async () => await EliminarSeguir(), () => true);
            GetPublicacionesUsuarioCommand = new Command(async () => await SeleccionarPublicacionesUsuario(), () => true);

            #endregion Comandos
        }
        #endregion Initialize

        #region Methods
        public async void ActualizarPerfil()
        {
            await SeleccionarPublicacionesUsuario();
            await SeleccionarSeguidores();
            await SeleccionarSeguidos();
            foreach (PeticionesSeguidos item in Usuario.Seguidores)
            {
                if (item.Apodo.Equals(UsuarioMemoria.Apodo))
                {
                    IsSeguirEnable = false;
                    break;
                }
            }

        }
        public async Task CrearSeguir()
        {
            try
            {
                PeticionesDosUsuariosModel peticion = new PeticionesDosUsuariosModel()
                {
                    idUsuario1 = UsuarioMemoria.idUsuario,
                    idUsuario2 = Usuario.idUsuario,
                };
                APIResponse response = await CreateSeguir.RunStrategy(peticion);
                if (response.IsSuccess)
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Publicacion eliminada exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al sequir al usuario";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task SeleccionarSeguidos()
        {
            ParametersRequest parametros = new ParametersRequest();
            parametros.Parameters.Add(Usuario.idUsuario.ToString());
            APIResponse response = await GetSeguidos.RunStrategy(null, parametros);
            if (response.IsSuccess)
            {
                List<PeticionesSeguidos> usuarios = JsonConvert.DeserializeObject<List<PeticionesSeguidos>>(response.Response);
                Usuario.Seguidos = usuarios;

            }
            else
            {
                ((MessageViewModel)PopUp.BindingContext).Message = "Error encontrar los seguidos del usuario";
                await PopupNavigation.Instance.PushAsync(PopUp);
                await Task.Delay(TimeSpan.FromSeconds(1));
                await PopupNavigation.Instance.PopAsync();
            }
        }

        public async Task SeleccionarSeguidores()
        {
            ParametersRequest parametros = new ParametersRequest();
            parametros.Parameters.Add(Usuario.idUsuario.ToString());
            APIResponse response = await GetSeguidores.RunStrategy(null, parametros);
            if (response.IsSuccess)
            {
                List<PeticionesSeguidos> usuarios = JsonConvert.DeserializeObject<List<PeticionesSeguidos>>(response.Response);
                Usuario.Seguidores = usuarios;

            }
            else
            {
                ((MessageViewModel)PopUp.BindingContext).Message = "Error encontrar los seguidores del usuario";
                await PopupNavigation.Instance.PushAsync(PopUp);
                await Task.Delay(TimeSpan.FromSeconds(1));
                await PopupNavigation.Instance.PopAsync();
            }
        }

        public async Task EliminarSeguir()
        {
            try
            {
                PeticionesDosUsuariosModel peticion = new PeticionesDosUsuariosModel()
                {
                    idUsuario1 = UsuarioMemoria.idUsuario,
                    idUsuario2 = Usuario.idUsuario,
                };
                APIResponse response = await DeleteSeguir.RunStrategy(peticion);
                if (response.IsSuccess)
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Publicacion eliminada exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    await PopupNavigation.Instance.PopAsync();
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al dejar de sequir al usuario";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    await PopupNavigation.Instance.PopAsync();
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task SeleccionarPublicacionesUsuario()
        {
            try
            {
                ParametersRequest parametros = new ParametersRequest();
                parametros.Parameters.Add(Usuario.idUsuario.ToString());
                APIResponse response = await GetPublicacionesUsuario.RunStrategy(null, parametros);
                if (response.IsSuccess)
                {
                    List<PublicacionModel> publicaciones = JsonConvert.DeserializeObject<List<PublicacionModel>>(response.Response);
                    Usuario.Publicaciones = publicaciones;
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "No se encuentran publicaciones del usuario";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    await PopupNavigation.Instance.PopAsync();
                }
            }
            catch (Exception e)
            {

            }
        }

        #endregion Methods

    }
}
