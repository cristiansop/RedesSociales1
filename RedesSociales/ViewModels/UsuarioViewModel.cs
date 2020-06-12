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
        public ICommand RefreshCommand { get; set; }
        public ICommand CreateSeguirCommand { get; set; }
        public ICommand GetSeguidosCommand { get; set; }
        public ICommand GetSeguidoresCommand { get; set; }
        public ICommand DeleteSeguirCommand { get; set; }
        public ICommand GetPublicacionesUsuarioCommand { get; set; }
        public ICommand FollowCommand { get; set; }
        

        #endregion Commands

        #endregion Properties

        #region Getters/Setters

        public UsuarioModel Usuario
        {
            get { return usuario; }
            set { usuario = value;
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
        public UsuarioViewModel(UsuarioModel usuario)
        {
            PopUp = new MessagePopupView();
            loadDataHandler = new LoadDataHandler();
            Usuario = usuario;
            UsuarioMemoria = (UsuarioModel)Application.Current.Properties["Usuario"];
            IsSeguirEnable =true;
            InitializeRequest();
            InitializeCommands();
            ActualizarPerfil();

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
            FollowCommand = new Command(async ()=>await Seguir(),()=>true);
            GetPublicacionesUsuarioCommand = new Command(async () => await SeleccionarPublicacionesUsuario(), () => true);
            RefreshCommand = new Command(() => ActualizarPerfil(), () => true);

            #endregion Comandos
        }
        #endregion Initialize

        #region Methods
        public async Task Seguir()
        {
            if (IsSeguirEnable)
            {
                await CrearSeguir();
            }
            else
            {
                await EliminarSeguir();
            }
            

        }
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
                    ActualizarPerfil();
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
                if (response.Code == 200)
                {
                    List<PeticionesSeguidos> usuarios = JsonConvert.DeserializeObject<List<PeticionesSeguidos>>(response.Response);
                    Usuario.Seguidos = usuarios;
                }
            }
        }

        public async Task SeleccionarSeguidores()
        {
            ParametersRequest parametros = new ParametersRequest();
            parametros.Parameters.Add(Usuario.idUsuario.ToString());
            APIResponse response = await GetSeguidores.RunStrategy(null, parametros);
            if (response.IsSuccess)
            {
                if (response.Code == 200)
                {
                    List<PeticionesSeguidos> usuarios = JsonConvert.DeserializeObject<List<PeticionesSeguidos>>(response.Response);
                    Usuario.Seguidores = usuarios;
                }
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
                    ActualizarPerfil();
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
                    if (response.Code == 200)
                    {
                        List<PublicacionModel> publicaciones = JsonConvert.DeserializeObject<List<PublicacionModel>>(response.Response);
                        Usuario.Publicaciones = publicaciones;
                    } 
                }
            }
            catch (Exception e)
            {

            }
        }

        public async void TraerPublicacionDetalle(PublicacionModel publicacion)
        {
            await NavigationService.PushPage(new ComentsView(publicacion));
        }

        #endregion Methods

    }
}
