using System;
using System.Collections.Generic;
using System.Text;
using RedesSociales.Configuracion;
using RedesSociales.Models;
using RedesSociales.Models.Auxiliary;
using RedesSociales.Servicios.Rest;
using RedesSociales.Servicios.Propagacion;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using RedesSociales.Views;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Services;
using RedesSociales.Validations.Base;
using Newtonsoft.Json;
using RedesSociales.Validations.Rules;
using RedesSociales.Servicios.Handler;

namespace RedesSociales.ViewModels
{
    public class PublicacionViewModel : ViewModelBase
    {
        #region Properties

        #region Atributes
        private LoadDataHandler loadDataHandler;

        public MessagePopupView PopUp { get; set; }
        public ObservableCollection<PublicacionModel> Publicaciones { get; set; }
        private PublicacionModel publicacion;
        private UsuarioModel usuario;
        public ValidatableObject<string> BusquedaUsuario { get; set; }
        public ValidatableObject<string> FotoPublicacion { get; set; }
        public ValidatableObject<string> TipoPublicacion { get; set; }
        public ValidatableObject<string> DescripcionPublicacion { get; set; }


        #endregion Atributes

        #region Request
        public SelectRequest<UsuarioModel> GetUsuario { get; set; }
        public SelectRequest<PublicacionModel> CreatePublicacion { get; set; }
        public SelectRequest<BaseModel> GetPublicacionesSeguidos { get; set; }
        public SelectRequest<BaseModel> GetPublicacionesUsuario { get; set; }
        public SelectRequest<PublicacionModel> DeletePublicacion { get; set; }
        #endregion Request

        #region Commands
        public ICommand GetUsuarioCommand { get; set; }
        public ICommand GetPublicacionesUsuarioCommand { get; set; }
        public ICommand CreatePublicacionCommand { get; set; }
        public ICommand GetPublicacionesSeguidosCommand { get; set; }
        public ICommand DeletePublicacionCommand { get; set; }
        #endregion Commands

        #endregion Properties
        #region Getters/Setters
        public PublicacionModel Publicacion
        {
            get { return publicacion; }
            set { publicacion = value; OnPropertyChanged(); }
        }
        public UsuarioModel Usuario
        {
            get { return usuario; }
            set { usuario = value; OnPropertyChanged(); }
        }
        #endregion Getters/Setters


        #region Initialize
        public PublicacionViewModel()
        {
            loadDataHandler = new LoadDataHandler();
            InitializeRequest();
            InitializeCommands();
            InitializeFields();
        }

        public void InitializeRequest()
        {
            #region Url
            string urlGetUsuario = Endpoints.URL_SERVIDOR + Endpoints.GET_USUARIO;
            string urlCreatePublicacion = Endpoints.URL_SERVIDOR + Endpoints.CREATE_PUBLICACION;
            string urlGetPublicacionesSeguidos = Endpoints.URL_SERVIDOR + Endpoints.GET_PUBLICACIONES_SEGUIDOS;
            string urlGetPublicacionesUsuario = Endpoints.URL_SERVIDOR + Endpoints.GET_PUBLICACIONES_USUARIO;
            string urlDeletePublicacion = Endpoints.URL_SERVIDOR + Endpoints.DELETE_PUBLICACIONES;
            
            #endregion Url
            #region API

            GetUsuario = new SelectRequest<UsuarioModel>();
            GetUsuario.SelectStrategy("GET", urlGetUsuario);

            DeletePublicacion = new SelectRequest<PublicacionModel>();
            DeletePublicacion.SelectStrategy("POST", urlDeletePublicacion);

            CreatePublicacion = new SelectRequest<PublicacionModel>();
            CreatePublicacion.SelectStrategy("POST", urlCreatePublicacion);

            GetPublicacionesSeguidos = new SelectRequest<BaseModel>();
            GetPublicacionesSeguidos.SelectStrategy("GET", urlGetPublicacionesSeguidos);

            GetPublicacionesUsuario = new SelectRequest<BaseModel>();
            GetPublicacionesUsuario.SelectStrategy("GET", urlGetPublicacionesUsuario);

            

            #endregion API
        }

        public void InitializeCommands()
        {
            GetUsuarioCommand = new Command(async () => await SeleccionarUsuario(), () => true);
            CreatePublicacionCommand = new Command(async () => await CrearPublicacion(), () => true);
            GetPublicacionesSeguidosCommand = new Command(async () => await SeleccionarPublicacionesSeguidos(), () => true);
            GetPublicacionesUsuarioCommand = new Command(async () => await SeleccionarPublicacionesUsuario(), () => true);
            DeletePublicacionCommand = new Command(async () => await EliminarPublicacion(), () => true);
        }

        public void InitializeFields()
        {
            FotoPublicacion = new ValidatableObject<string>();
            TipoPublicacion = new ValidatableObject<string>();
            DescripcionPublicacion = new ValidatableObject<string>();
            BusquedaUsuario = new ValidatableObject<string>();
            BusquedaUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El Apodo del usuario es obligatorio" });

        }
        #endregion Initialize
        private async Task SeleccionarUsuario()
        {
            try
            {
                ParametersRequest parametros = new ParametersRequest();
                parametros.Parameters.Add(BusquedaUsuario.Value);
                APIResponse response = await GetUsuario.RunStrategy(null, parametros);
                if (response.IsSuccess)
                {
                    Usuario = JsonConvert.DeserializeObject<UsuarioModel>(response.Response);
                    await SeleccionarPublicacionesUsuario();
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "No se encuentra el usuario";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {

            }
        }
        public async Task CrearPublicacion()
        {
            try
            {
                UsuarioModel Creador = (UsuarioModel)Application.Current.Properties["Usuario"];
                PublicacionModel publicacion = new PublicacionModel(Creador)
                {
                    Archivo = FotoPublicacion.Value,
                    Tipo=TipoPublicacion.Value,
                    Descripcion=DescripcionPublicacion.Value
                };
                APIResponse response = await CreatePublicacion.RunStrategy(publicacion);
                if (response.IsSuccess)
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Publicacion creada exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al crear publicacion";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task SeleccionarPublicacionesSeguidos()
        {
            try
            {
                UsuarioModel Creador = (UsuarioModel)Application.Current.Properties["Usuario"];
                ParametersRequest parametros = new ParametersRequest();
                parametros.Parameters.Add(Creador.idUsuario.ToString());
                APIResponse response = await GetPublicacionesUsuario.RunStrategy(null, parametros);
                if (response.IsSuccess)
                {
                    List<PublicacionModel> publicaciones = JsonConvert.DeserializeObject<List<PublicacionModel>>(response.Response);
                    Publicaciones = new ObservableCollection<PublicacionModel>(publicaciones);
                    
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "No se encuentran publicaciones del usuario";
                    await PopupNavigation.Instance.PushAsync(PopUp);
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
                    Publicaciones= new ObservableCollection<PublicacionModel>(publicaciones);
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "No se encuentran publicaciones del usuario";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task EliminarPublicacion()
        {
            try
            {
                UsuarioModel Creador = (UsuarioModel)Application.Current.Properties["Usuario"];
                PublicacionModel publicacion = new PublicacionModel(Creador)
                {
                    idPublicacion=Publicacion.idPublicacion
                };
                APIResponse response = await DeletePublicacion.RunStrategy(publicacion);
                if (response.IsSuccess)
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Publicacion eliminada exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al eliminar la publicacion";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
