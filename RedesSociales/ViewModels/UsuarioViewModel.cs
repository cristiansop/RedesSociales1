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
using System.IO;

namespace RedesSociales.ViewModels
{
    public class UsuarioViewModel : ViewModelBase
    {
        #region Properties

        #region Atributes
        private LoadDataHandler loadDataHandler;

        public MessagePopupView PopUp { get; set; }

        private UsuarioModel usuario;

        public UsuarioModel UsuarioMemoria { get; set; }

        private string salidaButton;
        private ObservableCollection<PublicacionUsuarioModel> publicaciones;

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

        public ObservableCollection<PublicacionUsuarioModel> Publicaciones
        {
            get { return publicaciones; }
            set { publicaciones = value; OnPropertyChanged(); }
        }

        public UsuarioModel Usuario
        {
            get { return usuario; }
            set
            {
                usuario = value;
                OnPropertyChanged();
            }
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
        public string SalidaButton
        {
            get { return salidaButton; }
            set
            {
                salidaButton = value;
                OnPropertyChanged();
            }
        }
        #endregion Getters/Setters

        #region Initialize
        public UsuarioViewModel()
        {
            PopUp = new MessagePopupView();
            loadDataHandler = new LoadDataHandler();
            Usuario = (UsuarioModel)Application.Current.Properties["UsuarioBusqueda"];
            UsuarioMemoria = (UsuarioModel)Application.Current.Properties["Usuario"];
            IsSeguirEnable = true;
            InitializeRequest();
            InitializeCommands();
            ActualizarPerfil();
            if(IsSeguirEnable) { SalidaButton = "Seguir"; }
            else { SalidaButton = "Siguiendo"; }

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
            FollowCommand = new Command(async () => await Seguir(), () => true);
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
                SalidaButton = "Siguiendo";
                IsSeguirEnable = false;
            }
            else
            {
                await EliminarSeguir();
                SalidaButton = "Seguir";
                IsSeguirEnable = true;
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
                        List<PublicacionUsuarioModel> publicaciones = JsonConvert.DeserializeObject<List<PublicacionUsuarioModel>>(response.Response);
                        Publicaciones = new ObservableCollection<PublicacionUsuarioModel>(publicaciones);
                        PublicacionUsuarioModel Publicacion = null;
                        for (int i = 0; i < Publicaciones.Count; i++)
                        {
                            Publicacion = Publicaciones[i];
                            byte[] imageBytes = Convert.FromBase64String(Publicacion.Archivo);
                            Publicacion.Imagen = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                            Publicaciones[i] = Publicacion;
                        }
                    }
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

        public async void TraerPublicacionDetalle(PublicacionModel publicacion)
        {
            await NavigationService.PushPage(new ComentsView());
        }

        #endregion Methods

    }
}
