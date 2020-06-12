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
using System.Linq;

namespace RedesSociales.ViewModels
{
    public class PublicacionViewModel : ViewModelBase
    {
        #region Properties

        #region Atributes

        private LoadDataHandler loadDataHandler;

        private ObservableCollection<PublicacionModel> publicaciones;

        private PublicacionModel publicacion;

        private UsuarioModel usuario;

        public MessagePopupView PopUp { get; set; }
        public UsuarioModel UsuarioMemoria { get; set; }
        public ValidatableObject<string> BusquedaUsuario { get; set; }
        public ValidatableObject<string> FotoPublicacion { get; set; }
        public ValidatableObject<string> TipoPublicacion { get; set; }
        public ValidatableObject<string> DescripcionPublicacion { get; set; }


        #endregion Atributes

        #region Request
        public SelectRequest<UsuarioModel> GetUsuario { get; set; }
        public SelectRequest<BaseModel> GetPublicacionesSeguidos { get; set; }
        public SelectRequest<BaseModel> GetLikes { get; set; }
        public SelectRequest<BaseModel> GetPublicacionesUsuario { get; set; }
        public SelectRequest<PublicacionModel> DeletePublicacion { get; set; }
        #endregion Request

        #region Commands
        public ICommand RefreshPublicacionCommand { get; set; }
        public ICommand GetLikesCommand { get; set; }
        public ICommand GetUsuarioCommand { get; set; }
        public ICommand GetPublicacionesUsuarioCommand { get; set; }
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
        public ObservableCollection<PublicacionModel> Publicaciones
        {
            get { return publicaciones; }
            set { publicaciones = value; OnPropertyChanged(); }
        }
        #endregion Getters/Setters

        #region Initialize
        public PublicacionViewModel()
        {
            loadDataHandler = new LoadDataHandler();
            UsuarioMemoria = (UsuarioModel)Application.Current.Properties["Usuario"];
            Publicaciones = new ObservableCollection<PublicacionModel>();

            ///
            UsuarioModel us = new UsuarioModel();
            us.Apodo = "la_loca";
            us.EstadoP = "Activo";
            us.NombreP = "Cristina";
            PublicacionModel pb = new PublicacionModel();
            pb.Creador = us;
            pb.Tiempo = "2020-05-25T04:50:14.148000000Z";
            pb.Descripcion = "Esta es la descripción de mi grandiosa publicación, \n Gracias!";
            Publicaciones.Add(pb);
            Publicaciones.Add(pb);
            ///

            Usuario = new UsuarioModel();
            InitializeRequest();
            InitializeCommands();
            InitializeFields();
            //TraerPublicaciones();


        }

        public void InitializeRequest()
        {
            #region Url
            string urlGetUsuario = Endpoints.URL_SERVIDOR + Endpoints.GET_USUARIO;
            string urlGetLikes = Endpoints.URL_SERVIDOR + Endpoints.GET_LIKES;
            string urlGetPublicacionesSeguidos = Endpoints.URL_SERVIDOR + Endpoints.GET_PUBLICACIONES_SEGUIDOS;
            string urlGetPublicacionesUsuario = Endpoints.URL_SERVIDOR + Endpoints.GET_PUBLICACIONES_USUARIO;
            string urlDeletePublicacion = Endpoints.URL_SERVIDOR + Endpoints.DELETE_PUBLICACIONES;
            
            #endregion Url
            #region API

            GetUsuario = new SelectRequest<UsuarioModel>();
            GetUsuario.SelectStrategy("GET", urlGetUsuario);

            DeletePublicacion = new SelectRequest<PublicacionModel>();
            DeletePublicacion.SelectStrategy("POST", urlDeletePublicacion);

            GetPublicacionesSeguidos = new SelectRequest<BaseModel>();
            GetPublicacionesSeguidos.SelectStrategy("GET", urlGetPublicacionesSeguidos);

            GetPublicacionesUsuario = new SelectRequest<BaseModel>();
            GetPublicacionesUsuario.SelectStrategy("GET", urlGetPublicacionesUsuario);

            GetLikes = new SelectRequest<BaseModel>();
            GetLikes.SelectStrategy("GET", urlGetLikes);

            #endregion API
        }

        public void InitializeCommands()
        {
            GetUsuarioCommand = new Command(async () => await SeleccionarUsuario(), () => true);
            GetLikesCommand = new Command(async () => await SeleccionarLikes(), () => true);
            GetPublicacionesSeguidosCommand = new Command(async () => await SeleccionarPublicacionesSeguidos(), () => true);
            GetPublicacionesUsuarioCommand = new Command(async () => await SeleccionarPublicacionesUsuario(), () => true);
            DeletePublicacionCommand = new Command(async () => await EliminarPublicacion(), () => true);
            RefreshPublicacionCommand = new Command(() =>  TraerPublicaciones(), () => true);
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

        #region Methods
        public async void TraerPublicaciones()
        {
            await SeleccionarPublicacionesSeguidos();            

        }

        public async Task SeleccionarUsuario()
        {
            try
            {
                ParametersRequest parametros = new ParametersRequest();
                parametros.Parameters.Add(BusquedaUsuario.Value);
                APIResponse response = await GetUsuario.RunStrategy(null, parametros);
                if (response.IsSuccess)
                {
                    if (response.Code == 200)
                    {
                        Usuario = JsonConvert.DeserializeObject<UsuarioModel>(response.Response);
                    }
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
        public async Task SeleccionarLikes()
        {
            ParametersRequest parametros = new ParametersRequest();
            parametros.Parameters.Add(Publicacion.idPublicacion.ToString());
            APIResponse response = await GetLikes.RunStrategy(null, parametros);
            if (response.IsSuccess)
            {
                if (response.Code == 200)
                {
                    List<PeticionesSeguidos> likes = JsonConvert.DeserializeObject<List<PeticionesSeguidos>>(response.Response);
                    Publicacion.Reacciones = likes;
                }
            }
            else
            {
                ((MessageViewModel)PopUp.BindingContext).Message = "Error encontrar las reacciones de la publicacion";
                await PopupNavigation.Instance.PushAsync(PopUp);
            }
        }

        public async Task SeleccionarPublicacionesSeguidos()
        {
            try
            {
                ParametersRequest parametros = new ParametersRequest();
                parametros.Parameters.Add(UsuarioMemoria.idUsuario.ToString());
                APIResponse response = await GetPublicacionesUsuario.RunStrategy(null, parametros);
                if (response.IsSuccess)
                {
                    if (response.Code == 200)
                    {
                        List<PublicacionModel> publicaciones = JsonConvert.DeserializeObject<List<PublicacionModel>>(response.Response);
                        ObservableCollection<PublicacionModel> publicaciones1 = new ObservableCollection<PublicacionModel>(publicaciones);
                        Publicaciones = new ObservableCollection<PublicacionModel>(Publicaciones.Union(publicaciones1).ToList());
                        foreach (PublicacionModel item in Publicaciones)
                        {
                            Publicacion = item;
                            await SeleccionarLikes();
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
                        Publicaciones = new ObservableCollection<PublicacionModel>(publicaciones);
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

        public async Task EliminarPublicacion()
        {
            try
            {
                PublicacionModel publicacion = new PublicacionModel(UsuarioMemoria)
                {
                    idPublicacion = Publicacion.idPublicacion
                };
                APIResponse response = await DeletePublicacion.RunStrategy(publicacion);
                if (response.IsSuccess)
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Publicacion eliminada exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    await PopupNavigation.Instance.PopAsync();
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al eliminar la publicacion";
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
