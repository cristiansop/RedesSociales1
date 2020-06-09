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
using RedesSociales.Views;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Services;
using RedesSociales.Validations.Base;
using Newtonsoft.Json;
using RedesSociales.Validations.Rules;

namespace RedesSociales.ViewModels
{
    public class PublicacionViewModel : ViewModelBase
    {
        #region Properties
        #region Atributes
        public MessagePopupView PopUp { get; set; }
        public ObservableCollection<PublicacionModel> Publicaciones { get; set; }
        private PublicacionModel publicacion;
        public UsuarioModel Creador { get; set; }
        private UsuarioModel usuario;
        public ValidatableObject<string> BusquedaUsuario { get; set; }
        public ValidatableObject<string> FotoPublicacion { get; set; }
        public ValidatableObject<string> TipoPublicacion { get; set; }
        public ValidatableObject<string> DescripcionPublicacion { get; set; }


        #endregion Atributes
        #region Request
        public ElegirRequest<UsuarioModel> GetUsuario { get; set; }
        public ElegirRequest<PublicacionModel> CreatePublicacion { get; set; }
        public ElegirRequest<BaseModel> GetPublicacionesSeguidos { get; set; }
        public ElegirRequest<BaseModel> GetPublicacionesUsuario { get; set; }
        public ElegirRequest<PublicacionModel> DeletePublicacion { get; set; }
        public ElegirRequest<PublicacionModel> CreateLike { get; set; }
        public ElegirRequest<BaseModel> GetLikes { get; set; }
        public ElegirRequest<PublicacionModel> DeleteLike { get; set; }
        public ElegirRequest<PublicacionModel> CreateEtiqueta { get; set; }
        public ElegirRequest<BaseModel> GetEtiquetas { get; set; }
        public ElegirRequest<PublicacionModel> DeleteEtiqueta { get; set; }
        #endregion Request
        #region Commands
        public ICommand GetUsuarioCommand { get; set; }
        public ICommand GetPublicacionesUsuarioCommand { get; set; }
        public ICommand CreatePublicacionCommand { get; set; }
        public ICommand GetPublicacionesSeguidosCommand { get; set; }
        public ICommand DeletePublicacionCommand { get; set; }
        public ICommand CreateLikeCommand { get; set; }
        public ICommand GetLikesCommand { get; set; }
        public ICommand DeleteLikeCommand { get; set; }
        public ICommand CreateEtiquetaCommand { get; set; }
        public ICommand GetEtiquetasCommand { get; set; }
        public ICommand DeleteEtiquetaCommand { get; set; }
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
            string urlCreateLike = Endpoints.URL_SERVIDOR + Endpoints.CREATE_LIKE;
            string urlGetLikes = Endpoints.URL_SERVIDOR + Endpoints.GET_LIKES;
            string urlDeleteLike = Endpoints.URL_SERVIDOR + Endpoints.DELETE_LIKE;
            string urlCreateEtiqueta = Endpoints.URL_SERVIDOR + Endpoints.CREATE_ETIQUETA;
            string urlGetEtiquetas = Endpoints.URL_SERVIDOR + Endpoints.GET_ETIQUETAS;
            string urlDeleteEtiqueta = Endpoints.URL_SERVIDOR + Endpoints.DELETE_ETIQUETA;
            #endregion Url
            #region API

            GetUsuario = new ElegirRequest<UsuarioModel>();
            GetUsuario.ElegirEstrategia("GET", urlGetUsuario);

            DeletePublicacion = new ElegirRequest<PublicacionModel>();
            DeletePublicacion.ElegirEstrategia("POST", urlDeletePublicacion);

            CreatePublicacion = new ElegirRequest<PublicacionModel>();
            CreatePublicacion.ElegirEstrategia("POST", urlCreatePublicacion);

            GetPublicacionesSeguidos = new ElegirRequest<BaseModel>();
            GetPublicacionesSeguidos.ElegirEstrategia("GET", urlGetPublicacionesSeguidos);

            GetPublicacionesUsuario = new ElegirRequest<BaseModel>();
            GetPublicacionesUsuario.ElegirEstrategia("GET", urlGetPublicacionesUsuario);

            CreateLike = new ElegirRequest<PublicacionModel>();
            CreateLike.ElegirEstrategia("POST", urlCreateLike);

            GetLikes = new ElegirRequest<BaseModel>();
            GetLikes.ElegirEstrategia("GET", urlGetLikes);

            DeleteLike = new ElegirRequest<PublicacionModel>();
            DeleteLike.ElegirEstrategia("POST", urlDeleteLike);

            CreateEtiqueta = new ElegirRequest<PublicacionModel>();
            CreateEtiqueta.ElegirEstrategia("POST", urlCreateEtiqueta);

            GetEtiquetas = new ElegirRequest<BaseModel>();
            GetEtiquetas.ElegirEstrategia("GET", urlGetEtiquetas);

            DeleteEtiqueta = new ElegirRequest<PublicacionModel>();
            DeleteEtiqueta.ElegirEstrategia("POST", urlDeleteEtiqueta);

            #endregion API
        }

        public void InitializeCommands()
        {
            GetUsuarioCommand = new Command(async () => await SeleccionarUsuario(), () => true);
            CreatePublicacionCommand = new Command(async () => await CrearPublicacion(), () => true);
            GetPublicacionesSeguidosCommand = new Command(async () => await SeleccionarPublicacionesSeguidos(), () => true);
            GetPublicacionesUsuarioCommand = new Command(async () => await SeleccionarPublicacionesUsuario(), () => true);
            DeletePublicacionCommand = new Command(async () => await EliminarPublicacion(), () => true);
            CreateLikeCommand = new Command(async () => await CrearLike(), () => true);
            GetLikesCommand = new Command(async () => await SeleccionarLikes(), () => true);
            DeleteLikeCommand = new Command(async () => await EliminarLike(), () => true);
            CreateEtiquetaCommand = new Command(async () => await CrearEtiqueta(), () => true);
            GetEtiquetasCommand = new Command(async () => await SeleccionarEtiquetas(), () => true);
            DeleteEtiquetaCommand = new Command(async () => await EliminarEtiqueta(), () => true);
        }

        public void InitializeFields()
        {
            FotoPublicacion = new ValidatableObject<string>();
            TipoPublicacion = new ValidatableObject<string>();
            DescripcionPublicacion = new ValidatableObject<string>();
            BusquedaUsuario = new ValidatableObject<string>();
            BusquedaUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El apodo del usuario es obligatorio" });

        }
        #endregion Initialize
        private async Task SeleccionarUsuario()
        {
            try
            {
                ParametersRequest parametros = new ParametersRequest();
                parametros.Parametros.Add(BusquedaUsuario.Value);
                APIResponse response = await GetUsuario.EjecutarEstrategia(null, parametros);
                if (response.IsSuccess)
                {
                    Usuario = JsonConvert.DeserializeObject<UsuarioModel>(response.Response);
                    
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
                PublicacionModel publicacion = new PublicacionModel(Creador)
                {
                    Imagen = FotoPublicacion.Value,
                    Tipo=TipoPublicacion.Value,
                    Descripcion=DescripcionPublicacion.Value
                };
                APIResponse response = await CreatePublicacion.EjecutarEstrategia(publicacion);
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
                ParametersRequest parametros = new ParametersRequest();
                parametros.Parametros.Add(Creador.Idusuario.ToString());
                APIResponse response = await GetPublicacionesUsuario.EjecutarEstrategia(null, parametros);
                if (response.IsSuccess)
                {
                    List<PublicacionModel> publicaciones = JsonConvert.DeserializeObject<List<PublicacionModel>>(response.Response);
                    Publicaciones = new ObservableCollection<PublicacionModel>(publicaciones);
                    foreach (PublicacionModel e in Publicaciones)
                    {
                        Publicacion = e;
                        await SeleccionarLikes();
                        await SeleccionarEtiquetas();
                    }
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
                parametros.Parametros.Add(Usuario.Idusuario.ToString());
                APIResponse response = await GetPublicacionesUsuario.EjecutarEstrategia(null, parametros);
                if (response.IsSuccess)
                {
                    List<PublicacionModel> publicaciones = JsonConvert.DeserializeObject<List<PublicacionModel>>(response.Response);
                    Publicaciones= new ObservableCollection<PublicacionModel>(publicaciones);
                    foreach (PublicacionModel e in Publicaciones)
                    {
                        Publicacion = e;
                        await SeleccionarLikes();
                        await SeleccionarEtiquetas();
                    }
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
                PublicacionModel publicacion = new PublicacionModel(Creador)
                {
                    IdPublicacion=Publicacion.IdPublicacion
                };
                APIResponse response = await DeletePublicacion.EjecutarEstrategia(publicacion);
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

        public async Task CrearLike()
        {
            try
            {
                PublicacionModel publicacion = new PublicacionModel(Creador)
                {
                    IdPublicacion = Publicacion.IdPublicacion
                };
                APIResponse response = await CreateLike.EjecutarEstrategia(publicacion);
                if (response.IsSuccess)
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Publicacion creada exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al reaccionar publicacion";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task SeleccionarLikes()
        {
            ParametersRequest parametros = new ParametersRequest();
            parametros.Parametros.Add(Publicacion.Idusuario.ToString());
            APIResponse response = await GetLikes.EjecutarEstrategia(null, parametros);
            if (response.IsSuccess)
            {
                List<UsuarioModel> usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(response.Response);
                Publicacion.Reacciones = usuarios;
            }
            else
            {
                ((MessageViewModel)PopUp.BindingContext).Message = "Error encontrar las reacciones de la publicacion";
                await PopupNavigation.Instance.PushAsync(PopUp);
            }
        }

        public async Task EliminarLike()
        {
            try
            {
                PublicacionModel publicacion = new PublicacionModel(Creador)
                {
                    IdPublicacion = Publicacion.IdPublicacion
                };
                APIResponse response = await DeleteLike.EjecutarEstrategia(publicacion);
                if (response.IsSuccess)
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Publicacion creada exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al reaccionar publicacion";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task CrearEtiqueta()
        {
            try
            {
                PublicacionModel publicacion = new PublicacionModel(Usuario)
                {
                    IdPublicacion = Publicacion.IdPublicacion
                };
                APIResponse response = await CreateEtiqueta.EjecutarEstrategia(publicacion);
                if (response.IsSuccess)
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Publicacion creada exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al reaccionar publicacion";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task SeleccionarEtiquetas()
        {
            ParametersRequest parametros = new ParametersRequest();
            parametros.Parametros.Add(Publicacion.Idusuario.ToString());
            APIResponse response = await GetEtiquetas.EjecutarEstrategia(null, parametros);
            if (response.IsSuccess)
            {
                List<UsuarioModel> usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(response.Response);
                Publicacion.Etiquetas = usuarios;
            }
            else
            {
                ((MessageViewModel)PopUp.BindingContext).Message = "Error encontrar las reacciones de la publicacion";
                await PopupNavigation.Instance.PushAsync(PopUp);
            }
        }

        public async Task EliminarEtiqueta()
        {
            try
            {
                PublicacionModel publicacion = new PublicacionModel(Usuario)
                {
                    IdPublicacion = Publicacion.IdPublicacion
                };
                APIResponse response = await CreateEtiqueta.EjecutarEstrategia(publicacion);
                if (response.IsSuccess)
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Publicacion creada exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al reaccionar publicacion";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
