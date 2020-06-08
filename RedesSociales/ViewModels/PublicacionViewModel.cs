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

namespace RedesSociales.ViewModels
{
    public class PublicacionViewModel : NotificationObject
    {
        #region Properties
        #region Atributes
        public MessagePopupView PopUp { get; set; }
        public ObservableCollection<PublicacionModel> Publicaciones { get; set; }
        private PublicacionModel publicacion;

        




        #endregion Atributes
        #region Request
        public ElegirRequest<PublicacionModel> CreatePublicacion { get; set; }
        public ElegirRequest<BaseModel> GetPublicacionesSeguidos { get; set; }
        public ElegirRequest<BaseModel> GetPublicacionesUsuario { get; set; }
        public ElegirRequest<BaseModel> DeletePublicacion { get; set; }
        public ElegirRequest<BaseModel> CreateLike { get; set; }
        public ElegirRequest<BaseModel> GetLikes { get; set; }
        public ElegirRequest<BaseModel> DeleteLike { get; set; }
        public ElegirRequest<BaseModel> CreateEtiqueta { get; set; }
        public ElegirRequest<BaseModel> GetEtiquetas { get; set; }
        public ElegirRequest<BaseModel> DeleteEtiqueta { get; set; }
        #endregion Request
        #region Commands
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

            DeletePublicacion = new ElegirRequest<BaseModel>();
            DeletePublicacion.ElegirEstrategia("POST", urlDeletePublicacion);

            CreatePublicacion = new ElegirRequest<PublicacionModel>();
            CreatePublicacion.ElegirEstrategia("POST", urlCreatePublicacion);

            GetPublicacionesSeguidos = new ElegirRequest<BaseModel>();
            GetPublicacionesSeguidos.ElegirEstrategia("GET", urlGetPublicacionesSeguidos);

            GetPublicacionesUsuario = new ElegirRequest<BaseModel>();
            GetPublicacionesUsuario.ElegirEstrategia("GET", urlGetPublicacionesUsuario);

            CreateLike = new ElegirRequest<BaseModel>();
            CreateLike.ElegirEstrategia("POST", urlCreateLike);

            GetLikes = new ElegirRequest<BaseModel>();
            GetLikes.ElegirEstrategia("GET", urlGetLikes);

            DeleteLike = new ElegirRequest<BaseModel>();
            DeleteLike.ElegirEstrategia("POST", urlDeleteLike);

            CreateEtiqueta = new ElegirRequest<BaseModel>();
            CreateEtiqueta.ElegirEstrategia("POST", urlCreateEtiqueta);

            GetEtiquetas = new ElegirRequest<BaseModel>();
            GetEtiquetas.ElegirEstrategia("GET", urlGetEtiquetas);

            DeleteEtiqueta = new ElegirRequest<BaseModel>();
            DeleteEtiqueta.ElegirEstrategia("POST", urlDeleteEtiqueta);

            #endregion API
        }

        public void InitializeCommands()
        {
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
            throw new NotImplementedException();
        }
        #endregion Initialize
        public async Task CrearPublicacion()
        {
            throw new NotImplementedException();
        }

        public async Task SeleccionarPublicacionesSeguidos()
        {
            throw new NotImplementedException();
        }

        public async Task SeleccionarPublicacionesUsuario()
        {
            throw new NotImplementedException();
        }

        public async Task EliminarPublicacion()
        {
            throw new NotImplementedException();
        }

        public async Task CrearLike()
        {
            throw new NotImplementedException();
        }

        public async Task SeleccionarLikes()
        {
            throw new NotImplementedException();
        }

        public async Task EliminarLike()
        {
            throw new NotImplementedException();
        }

        public async Task CrearEtiqueta()
        {
            throw new NotImplementedException();
        }

        public async Task SeleccionarEtiquetas()
        {
            throw new NotImplementedException();
        }

        public async Task EliminarEtiqueta()
        {
            throw new NotImplementedException();
        }
    }
}
