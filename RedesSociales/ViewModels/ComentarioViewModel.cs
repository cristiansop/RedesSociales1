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
        public ValidatableObject<string> CuerpoEntry { get; set; }

        #region Enables
        #endregion Enables

        #endregion Atributes

        #region Request

        public SelectRequest<ComentarioModel> CreateComentario { get; set; }
        public SelectRequest<BaseModel> GetComentarios { get; set; }
        public SelectRequest<ComentarioModel> DeleteComentario { get; set; }

        #endregion Request

        #region Commands
        public ICommand CreateComentarioCommand { get; set; }
        public ICommand GetComentariosCommand { get; set; }
        public ICommand DeleteComentarioCommand { get; set; }

        #endregion Commands

        #endregion Properties

        #region Getters/Setters

        #endregion Getters/Setters

        #region Initialize
        public ComentarioViewModel()
        {
            PopUp = new MessagePopupView();
            Usuario = new UsuarioModel();
            Publicacion = new PublicacionModel();
            InitializeRequest();
            InitializeCommands();
            InitializeFields();
        }
        public void InitializeRequest()
        {
            #region Url
            string urlCreateComentario = Endpoints.URL_SERVIDOR + Endpoints.CREATE_COMENTARIO;
            string urlGetComentarios = Endpoints.URL_SERVIDOR + Endpoints.GET_COMENTARIOS;
            string urlDeleteComentario = Endpoints.URL_SERVIDOR + Endpoints.DELETE_COMENTARIO;
            #endregion Url

            #region API
            CreateComentario = new SelectRequest<ComentarioModel>();
            CreateComentario.SelectStrategy("POST", urlCreateComentario);

            GetComentarios = new SelectRequest<BaseModel>();
            GetComentarios.SelectStrategy("GET", urlGetComentarios);

            DeleteComentario = new SelectRequest<ComentarioModel>();
            DeleteComentario.SelectStrategy("GET", urlDeleteComentario);

            #endregion API
        }
        public void InitializeCommands()
        {
            #region Comandos

            CreateComentarioCommand = new Command(async () => await CrearComentario(), () => true);
            GetComentariosCommand = new Command(async () => await SeleccionarComentarios(), () => true);
            DeleteComentarioCommand = new Command(async () => await EliminarComentario(), () => true);

            #endregion Comandos
        }



        public void InitializeFields()
        {
            CuerpoEntry = new ValidatableObject<string>();

            CuerpoEntry.Validations.Add(new RequiredRule<string> { ValidationMessage = "El Cuerpo del comentario es obligatorio" });

        }
        #endregion Initialize

        #region Methods
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

        public async Task SeleccionarComentarios()
        {
            try
            {
                ParametersRequest parametros = new ParametersRequest();
                parametros.Parameters.Add(Publicacion.IdPublicacion.ToString());
                APIResponse response = await GetComentarios.RunStrategy(null, parametros);
                if (response.IsSuccess)
                {
                    List<ComentarioModel> comentarios = JsonConvert.DeserializeObject<List<ComentarioModel>>(response.Response);
                    Publicacion.Comentarios = comentarios;
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

        #endregion Methods

    }
}