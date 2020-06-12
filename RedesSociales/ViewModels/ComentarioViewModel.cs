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
using RedesSociales.Validations.Base;
using RedesSociales.Validations.Rules;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace RedesSociales.ViewModels
{
    class ComentarioViewModel : ViewModelBase
    {
        #region Properties

        #region Atributes

        public MessagePopupView PopUp { get; set; }
        public UsuarioModel Usuario { get; set; }
        public PublicacionModel Publicacion { get; set; }
        private ComentarioModel comentario;



        public ValidatableObject<string> CuerpoEntry { get; set; }
        private bool like;

        #region Enables

        private bool isCommentEnable;
        private bool isCreatorEnable;
        private bool isCommentCreatorEnable;

        #endregion Enables

        #endregion Atributes

        #region Request

        public SelectRequest<ComentarioModel> CreateComentario { get; set; }
        public SelectRequest<BaseModel> GetComentarios { get; set; }
        public SelectRequest<ComentarioModel> DeleteComentario { get; set; }
        public SelectRequest<PeticionesUsuarioPublicacion> CreateLike { get; set; }
        public SelectRequest<BaseModel> GetLikes { get; set; }
        public SelectRequest<PeticionesUsuarioPublicacion> DeleteLike { get; set; }
        public SelectRequest<PeticionesUsuarioPublicacion> CreateEtiqueta { get; set; }
        public SelectRequest<BaseModel> GetEtiquetas { get; set; }
        public SelectRequest<PeticionesUsuarioPublicacion> DeleteEtiqueta { get; set; }
        public SelectRequest<PublicacionModel> DeletePublicacion { get; set; }
        #endregion Request

        #region Commands
        public ICommand CreateComentarioCommand { get; set; }
        public ICommand GetComentariosCommand { get; set; }
        public ICommand DeleteComentarioCommand { get; set; }
        public ICommand CreateLikeCommand { get; set; }
        public ICommand GetLikesCommand { get; set; }
        public ICommand DeleteLikeCommand { get; set; }
        public ICommand CreateEtiquetaCommand { get; set; }
        public ICommand GetEtiquetasCommand { get; set; }
        public ICommand DeleteEtiquetaCommand { get; set; }
        public ICommand DeletePublicacionCommand { get; set; }
        public ICommand LikeCommand { get; set; }
        public ICommand RefreshCommand { get; set; }


        #endregion Commands

        #endregion Properties

        #region Getters/Setters
        public bool Like
        {
            get { return like; }
            set { like = value; OnPropertyChanged(); }
        }

        public bool IsCommentEnable
        {
            get { return isCommentEnable; }
            set { isCommentEnable = value; OnPropertyChanged(); }
        }
        public bool IsCreatorEnable
        {
            get { return isCreatorEnable; }
            set { isCreatorEnable = value; OnPropertyChanged(); }
        }
        public bool IsCommentCreatorEnable
        {
            get { return isCommentCreatorEnable; }
            set { isCommentCreatorEnable = value; OnPropertyChanged(); }
        }
        public ComentarioModel Comentario
        {
            get { return comentario; }
            set
            {
                comentario = value;
                IsCommentCreatorEnable = IsCreatorEnable || comentario.idUsuario == Usuario.idUsuario;
                OnPropertyChanged();
            }
        }
        #endregion Getters/Setters

        #region Initialize
        public ComentarioViewModel()
        {
            PopUp = new MessagePopupView();
            Usuario = (UsuarioModel)Application.Current.Properties["Usuario"];
            Publicacion = (PublicacionModel)Application.Current.Properties["Publicacion"];
            Like = false;
            IsCommentEnable = false;
            IsCreatorEnable = Publicacion.Apodo.Equals(Usuario.Apodo);
            IsCommentCreatorEnable = false;
            InitializeRequest();
            InitializeCommands();
            InitializeFields();
            AddValidations();
            TraerPublicacion();
        }
        public void InitializeRequest()
        {
            #region Url
            string urlCreateComentario = Endpoints.URL_SERVIDOR + Endpoints.CREATE_COMENTARIO;
            string urlGetComentarios = Endpoints.URL_SERVIDOR + Endpoints.GET_COMENTARIOS;
            string urlDeleteComentario = Endpoints.URL_SERVIDOR + Endpoints.DELETE_COMENTARIO;
            string urlCreateLike = Endpoints.URL_SERVIDOR + Endpoints.CREATE_LIKE;
            string urlGetLikes = Endpoints.URL_SERVIDOR + Endpoints.GET_LIKES;
            string urlDeleteLike = Endpoints.URL_SERVIDOR + Endpoints.DELETE_LIKE;
            string urlCreateEtiqueta = Endpoints.URL_SERVIDOR + Endpoints.CREATE_ETIQUETA;
            string urlGetEtiquetas = Endpoints.URL_SERVIDOR + Endpoints.GET_ETIQUETAS;
            string urlDeleteEtiqueta = Endpoints.URL_SERVIDOR + Endpoints.DELETE_ETIQUETA;
            string urlDeletePublicacion = Endpoints.URL_SERVIDOR + Endpoints.DELETE_PUBLICACIONES;
            #endregion Url

            #region API
            CreateComentario = new SelectRequest<ComentarioModel>();
            CreateComentario.SelectStrategy("POST", urlCreateComentario);

            GetComentarios = new SelectRequest<BaseModel>();
            GetComentarios.SelectStrategy("GET", urlGetComentarios);

            DeleteComentario = new SelectRequest<ComentarioModel>();
            DeleteComentario.SelectStrategy("GET", urlDeleteComentario);

            CreateLike = new SelectRequest<PeticionesUsuarioPublicacion>();
            CreateLike.SelectStrategy("POST", urlCreateLike);

            GetLikes = new SelectRequest<BaseModel>();
            GetLikes.SelectStrategy("GET", urlGetLikes);

            DeleteLike = new SelectRequest<PeticionesUsuarioPublicacion>();
            DeleteLike.SelectStrategy("POST", urlDeleteLike);

            CreateEtiqueta = new SelectRequest<PeticionesUsuarioPublicacion>();
            CreateEtiqueta.SelectStrategy("POST", urlCreateEtiqueta);

            GetEtiquetas = new SelectRequest<BaseModel>();
            GetEtiquetas.SelectStrategy("GET", urlGetEtiquetas);

            DeleteEtiqueta = new SelectRequest<PeticionesUsuarioPublicacion>();
            DeleteEtiqueta.SelectStrategy("POST", urlDeleteEtiqueta);

            DeletePublicacion = new SelectRequest<PublicacionModel>();
            DeletePublicacion.SelectStrategy("POST", urlDeletePublicacion);
            #endregion API
        }
        public void InitializeCommands()
        {
            #region Comandos

            CreateComentarioCommand = new Command(async () => await CrearComentario(), () => IsCommentEnable);
            GetComentariosCommand = new Command(async () => await SeleccionarComentarios(), () => true);
            DeleteComentarioCommand = new Command(async () => await EliminarComentario(), () => IsCommentCreatorEnable);
            LikeCommand = new Command(async () => await DarLike(), () => true);
            CreateLikeCommand = new Command(async () => await CrearLike(), () => true);
            GetLikesCommand = new Command(async () => await SeleccionarLikes(), () => true);
            DeleteLikeCommand = new Command(async () => await EliminarLike(), () => true);
            CreateEtiquetaCommand = new Command(async () => await CrearEtiqueta(), () => IsCreatorEnable);
            GetEtiquetasCommand = new Command(async () => await SeleccionarEtiquetas(), () => true);
            DeleteEtiquetaCommand = new Command(async () => await EliminarEtiqueta(), () => IsCreatorEnable);
            DeletePublicacionCommand = new Command(async () => await EliminarPublicacion(), () => IsCreatorEnable);
            RefreshCommand = new Command(() => TraerPublicacion(), () => true);

            #endregion Comandos
        }



        public void InitializeFields()
        {
            CuerpoEntry = new ValidatableObject<string>();

        }
        public void AddValidations()
        {
            CuerpoEntry.Validations.Add(new RequiredRule<string> { ValidationMessage = "El Cuerpo del comentario es obligatorio" });
        }

        #endregion Initialize

        #region Methods
        private void ValidateForm()
        {
            bool CuerpoValidate = CuerpoEntry.Validate();
            IsCommentEnable = CuerpoValidate;
            ((Command)CreateComentarioCommand).ChangeCanExecute();
        }
        public async Task DarLike()
        {
            if (Like)
            {
                await EliminarLike();
                Like = false;
            }
            else
            {
                await CrearLike();
                Like = true;
            }
        }
        public async void TraerPublicacion()
        {
            await SeleccionarComentarios();
            await SeleccionarEtiquetas();
            await SeleccionarLikes();
            foreach (PeticionesSeguidos item in Publicacion.Reacciones)
            {
                if (item.Apodo.Equals(Usuario.Apodo))
                {
                    Like = true;
                    break;
                }
            }

        }
        public async Task CrearComentario()
        {
            try
            {
                ComentarioModel peticion = new ComentarioModel(Usuario, Publicacion)
                {
                    Cuerpo = CuerpoEntry.Value
                };
                APIResponse response = await CreateComentario.RunStrategy(peticion);
                if (response.IsSuccess)
                {
                    TraerPublicacion();
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task SeleccionarComentarios()
        {
            try
            {
                ParametersRequest parametros = new ParametersRequest();
                parametros.Parameters.Add(Publicacion.idPublicacion.ToString());
                APIResponse response = await GetComentarios.RunStrategy(null, parametros);
                if (response.IsSuccess)
                {
                    if (response.Code == 200)
                    {
                        List<PeticionesComentariosPublicacion> comentarios = JsonConvert.DeserializeObject<List<PeticionesComentariosPublicacion>>(response.Response);
                        Publicacion.Comentarios = comentarios;
                    }
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "No se encuentran comentarios de la publicacion";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task EliminarComentario()
        {
            try
            {
                ComentarioModel peticion = new ComentarioModel(Usuario, Publicacion)
                {
                    Cuerpo = CuerpoEntry.Value
                };
                APIResponse response = await DeleteComentario.RunStrategy(peticion);
                if (response.IsSuccess)
                {
                    TraerPublicacion();
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
                PeticionesUsuarioPublicacion peticion = new PeticionesUsuarioPublicacion()
                {
                    idUsuario = Usuario.idUsuario,
                    idPublicacion = Publicacion.idPublicacion
                };
                APIResponse response = await CreateLike.RunStrategy(peticion);
                if (response.IsSuccess)
                {
                    //isenablechange
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
        }

        public async Task EliminarLike()
        {
            try
            {
                UsuarioModel Creador = (UsuarioModel)Application.Current.Properties["Usuario"];
                PeticionesUsuarioPublicacion peticion = new PeticionesUsuarioPublicacion()
                {
                    idUsuario = Creador.idUsuario,
                    idPublicacion = Publicacion.idPublicacion
                };
                APIResponse response = await DeleteLike.RunStrategy(peticion);
                if (response.IsSuccess)
                {
                    //isenablechange
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
                PeticionesUsuarioPublicacion peticion = new PeticionesUsuarioPublicacion()
                {
                    idUsuario = Usuario.idUsuario,
                    idPublicacion = Publicacion.idPublicacion
                };
                APIResponse response = await CreateEtiqueta.RunStrategy(peticion);
                if (response.IsSuccess)
                {
                    TraerPublicacion();
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task SeleccionarEtiquetas()
        {
            ParametersRequest parametros = new ParametersRequest();
            parametros.Parameters.Add(Publicacion.idPublicacion.ToString());
            APIResponse response = await GetEtiquetas.RunStrategy(null, parametros);
            if (response.IsSuccess)
            {
                if (response.Code == 200)
                {
                    List<PeticionesSeguidos> usuarios = JsonConvert.DeserializeObject<List<PeticionesSeguidos>>(response.Response);
                    Publicacion.Etiquetas = usuarios;
                }
            }
        }

        public async Task EliminarEtiqueta()
        {
            try
            {
                //cambiar
                UsuarioModel Creador = (UsuarioModel)Application.Current.Properties["Usuario"];
                PeticionesUsuarioPublicacion peticion = new PeticionesUsuarioPublicacion()
                {
                    idUsuario = Creador.idUsuario,
                    idPublicacion = Publicacion.idPublicacion
                };
                APIResponse response = await CreateEtiqueta.RunStrategy(peticion);
                if (response.IsSuccess)
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Etiqueta creada exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al eliminar Etiqueta";
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
                PublicacionModel publicacion = new PublicacionModel(Usuario)
                {
                    idPublicacion = Publicacion.idPublicacion
                };
                APIResponse response = await DeletePublicacion.RunStrategy(publicacion);
                if (response.IsSuccess)
                {

                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion Methods

    }
}