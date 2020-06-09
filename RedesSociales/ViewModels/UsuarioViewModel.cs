using RedesSociales.Configuracion;
using RedesSociales.Models;
using RedesSociales.Models.AuxiliarModels;
using RedesSociales.Servicios.APIRest;
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

namespace RedesSociales.ViewModels
{
    public class UsuarioViewModel: ViewModelBase
    {
        #region Properties

        #region Atributes

        public MessagePopupView PopUp { get; set; }
        private UsuarioModel usuario;
        public PublicacionViewModel Publicacionviewmodel { get; set; }
        public ValidatableObject<string> ApodoUsuario { get; set; }
        public ValidatableObject<string> NombreUsuario { get; set; }
        public ValidatableObject<string> ApellidosUsuario { get; set; }
        public ValidatableObject<string> FotoPerfilUsuario { get; set; }
        public ValidatableObject<string> EstadoUsuario { get; set; }

        #region Enables
        #endregion Enables

        #endregion Atributes

        #region Request
        public ElegirRequest<UsuarioModel> UpdateUsuario { get; set; }
        public ElegirRequest<BaseModel> DeleteUsuario { get; set; }
        public ElegirRequest<PeticionesDosUsuariosModel> CreateSeguir { get; set; }
        public ElegirRequest<BaseModel> GetSeguidos { get; set; }
        public ElegirRequest<BaseModel> GetSeguidores { get; set; }
        public ElegirRequest<PeticionesDosUsuariosModel> DeleteSeguir { get; set; }

        #endregion Request

        #region Commands
        public ICommand UpdateUsuarioCommand { get; set; }
        public ICommand DeleteUsuarioCommand { get; set; }
        public ICommand CreateSeguirCommand { get; set; }
        public ICommand GetSeguidosCommand { get; set; }
        public ICommand GetSeguidoresCommand { get; set; }
        public ICommand DeleteSeguirCommand { get; set; }

        #endregion Commands

        #endregion Properties

        #region Getters/Setters

        public UsuarioModel Usuario
        {
            get { return usuario; }
            set { usuario = value;
                OnPropertyChanged(); }
        }

        #endregion Getters/Setters

        #region Initialize
        public UsuarioViewModel()
        {
            PopUp = new MessagePopupView();
            Usuario = new UsuarioModel();
            Publicacionviewmodel = new PublicacionViewModel();
            InitializeRequest();
            InitializeCommands();
            InitializeFields();
        }
        public void InitializeRequest()
        {
            #region Url
            string urlUpdateUsuario = Endpoints.URL_SERVIDOR + Endpoints.UPDATE_USUARIO;
            string urlDeleteUsuario = Endpoints.URL_SERVIDOR + Endpoints.DELETE_USUARIO;
            string urlCreateSeguir = Endpoints.URL_SERVIDOR + Endpoints.CREATE_SEGUIR;
            string urlGetSeguidos = Endpoints.URL_SERVIDOR + Endpoints.GET_SEGUIDOS;
            string urlGetSeguidores = Endpoints.URL_SERVIDOR + Endpoints.GET_SEGUIDORES;
            string urlDeleteSeguir = Endpoints.URL_SERVIDOR + Endpoints.DELETE_SEGUIR;
            #endregion Url

            #region API

            UpdateUsuario = new ElegirRequest<UsuarioModel>();
            UpdateUsuario.ElegirEstrategia("POST", urlUpdateUsuario);

            DeleteUsuario = new ElegirRequest<BaseModel>();
            DeleteUsuario.ElegirEstrategia("POST", urlDeleteUsuario);

            CreateSeguir = new ElegirRequest<PeticionesDosUsuariosModel>();
            CreateSeguir.ElegirEstrategia("POST", urlCreateSeguir);

            GetSeguidos = new ElegirRequest<BaseModel>();
            GetSeguidos.ElegirEstrategia("GET", urlGetSeguidos);

            GetSeguidores = new ElegirRequest<BaseModel>();
            GetSeguidores.ElegirEstrategia("GET", urlGetSeguidores);

            DeleteSeguir = new ElegirRequest<PeticionesDosUsuariosModel>();
            DeleteSeguir.ElegirEstrategia("POST", urlDeleteSeguir);
            #endregion API
        }
        public void InitializeCommands()
        {
            #region Comandos
            
            UpdateUsuarioCommand = new Command(async () => await ActualizarUsuario(), () => true);
            DeleteUsuarioCommand = new Command(async () => await EliminarUsuario(), () => true);
            CreateSeguirCommand = new Command(async () => await CrearSeguir(), () => true);
            GetSeguidosCommand = new Command(async () => await SeleccionarSeguidos(), () => true);
            GetSeguidoresCommand = new Command(async () => await SeleccionarSeguidores(), () => true);
            DeleteSeguirCommand = new Command(async () => await EliminarSeguir(), () => true);

            #endregion Comandos
        }

        public void InitializeFields()
        {
            ApodoUsuario = new ValidatableObject<string>();
            NombreUsuario = new ValidatableObject<string>();
            ApellidosUsuario = new ValidatableObject<string>();
            FotoPerfilUsuario = new ValidatableObject<string>();
            EstadoUsuario = new ValidatableObject<string>();
            

            ApodoUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El apodo del usuario es obligatorio" });
            NombreUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El nombre del usuario es obligatorio" });
            ApellidosUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "los apellidos del usuario es obligatorio" });
            EstadoUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El Estado de la cuenta es obligatorio" });
            
        }
        #endregion Initialize

        #region Methods

        public async Task ActualizarUsuario()
        {
            try
            {
                UsuarioModel usuario = new UsuarioModel()
                {
                    apodo = ApodoUsuario.Value,
                    Nombre = NombreUsuario.Value,
                    Apellidos = ApellidosUsuario.Value,
                    FotoPerfil = FotoPerfilUsuario.Value,
                    Estado = EstadoUsuario.Value
                };
                APIResponse response = await UpdateUsuario.EjecutarEstrategia(usuario);
                if (response.IsSuccess)
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Categoría actualizada exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al actualizar la categoría";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task EliminarUsuario()
        {
            try
            {
                UsuarioModel usuario1 = new UsuarioModel()
                {
                    Idusuario = Usuario.Idusuario
                };
                APIResponse response = await DeleteUsuario.EjecutarEstrategia(usuario1);
                if (response.IsSuccess)
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Usuario eliminado exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al eliminar la categoría";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task CrearSeguir()
        {
            try
            {
                PeticionesDosUsuariosModel peticion = new PeticionesDosUsuariosModel()
                {
                    Idusuario1 = usuario.Idusuario,
                    Idusuario2 = usuario1.Idusuario,
                };
                APIResponse response = await CreateSeguir.EjecutarEstrategia(peticion);
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
            parametros.Parametros.Add(Usuario.Idusuario.ToString());
            APIResponse response = await GetSeguidos.EjecutarEstrategia(null, parametros);
            if (response.IsSuccess)
            {
                List<UsuarioModel> usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(response.Response);
                Usuario.Seguidos = usuarios;

            }
            else
            {
                ((MessageViewModel)PopUp.BindingContext).Message = "Error encontrar los seguidos del usuario";
                await PopupNavigation.Instance.PushAsync(PopUp);
            }
        }

        public async Task SeleccionarSeguidores()
        {
            ParametersRequest parametros = new ParametersRequest();
            parametros.Parametros.Add(Usuario.Idusuario.ToString());
            APIResponse response = await GetSeguidores.EjecutarEstrategia(null, parametros);
            if (response.IsSuccess)
            {
                List<UsuarioModel> usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(response.Response);
                Usuario.Seguidores = usuarios;

            }
            else
            {
                ((MessageViewModel)PopUp.BindingContext).Message = "Error encontrar los seguidores del usuario";
                await PopupNavigation.Instance.PushAsync(PopUp);
            }
        }

        public async Task EliminarSeguir()
        {
            try
            {
                PeticionesDosUsuariosModel peticion = new PeticionesDosUsuariosModel()
                {
                    Idusuario1 = usuario.Idusuario,
                    Idusuario2 = usuario.Idusuario,
                };
                APIResponse response = await DeleteSeguir.EjecutarEstrategia(peticion);
                if (response.IsSuccess)
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Publicacion eliminada exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al dejar de sequir al usuario";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {

            }
        }
        public async Task IniciarUsuario()
        {
            
        }
        

        #endregion Methods

    }
}
