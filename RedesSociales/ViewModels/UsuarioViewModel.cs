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
        public PublicacionViewModel Publicacionviewmodel { get; set; }
        public ValidatableObject<string> ApodoUsuario { get; set; }
        public ValidatableObject<string> NombreUsuario { get; set; }
        public ValidatableObject<string> ApellidosUsuario { get; set; }
        public ValidatableObject<string> FotoPerfilUsuario { get; set; }
        public ValidatableObject<string> EstadoUsuario { get; set; }

        #region Enables
        private bool isModificarEnable;
        private bool isEliminarEnable;
        private bool isSeguirEnable;
        private bool isCrearPublicacionEnable;
        private bool isDeletePublicacionEnable;

        public bool IsDeletePublicacionEnable
        {
            get { return isDeletePublicacionEnable; }
            set { isDeletePublicacionEnable = value; }
        }


        public bool IsCrearPublicacionEnable
        {
            get { return isCrearPublicacionEnable; }
            set { isCrearPublicacionEnable = value; }
        }


        #endregion Enables

        #endregion Atributes

        #region Request
        public SelectRequest<UsuarioModel> UpdateUsuario { get; set; }
        public SelectRequest<BaseModel> DeleteUsuario { get; set; }
        public SelectRequest<PeticionesDosUsuariosModel> CreateSeguir { get; set; }
        public SelectRequest<BaseModel> GetSeguidos { get; set; }
        public SelectRequest<BaseModel> GetSeguidores { get; set; }
        public SelectRequest<PeticionesDosUsuariosModel> DeleteSeguir { get; set; }

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
        public bool IsSeguirEnable
        {
            get { return isSeguirEnable; }
            set
            {
                isSeguirEnable = value;
                OnPropertyChanged();
            }
        }


        public bool IsEliminarEnable
        {
            get { return isEliminarEnable; }
            set
            {
                isEliminarEnable = value;
                OnPropertyChanged();
            }
        }


        public bool IsModificarEnable
        {
            get { return isModificarEnable; }
            set
            {
                isModificarEnable = value;
                OnPropertyChanged();
            }
        }

        #endregion Getters/Setters

        #region Initialize
        public UsuarioViewModel()
        {
            PopUp = new MessagePopupView();
            Usuario = new UsuarioModel();
            IsModificarEnable=false;
            IsEliminarEnable=false;
            IsSeguirEnable=true;
            loadDataHandler = new LoadDataHandler();
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

            UpdateUsuario = new SelectRequest<UsuarioModel>();
            UpdateUsuario.SelectStrategy("POST", urlUpdateUsuario);

            DeleteUsuario = new SelectRequest<BaseModel>();
            DeleteUsuario.SelectStrategy("POST", urlDeleteUsuario);

            CreateSeguir = new SelectRequest<PeticionesDosUsuariosModel>();
            CreateSeguir.SelectStrategy("POST", urlCreateSeguir);

            GetSeguidos = new SelectRequest<BaseModel>();
            GetSeguidos.SelectStrategy("GET", urlGetSeguidos);

            GetSeguidores = new SelectRequest<BaseModel>();
            GetSeguidores.SelectStrategy("GET", urlGetSeguidores);

            DeleteSeguir = new SelectRequest<PeticionesDosUsuariosModel>();
            DeleteSeguir.SelectStrategy("POST", urlDeleteSeguir);
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
            

            ApodoUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El Apodo del usuario es obligatorio" });
            NombreUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El nombre del usuario es obligatorio" });
            ApellidosUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "los apellidos del usuario es obligatorio" });
            EstadoUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El Estado de la cuenta es obligatorio" });
            
        }
        #endregion Initialize

        #region Methods
        public void ActualizarComandos()
        {
            UsuarioModel UsuarioMemoria=(UsuarioModel)Application.Current.Properties["Usuario"];
            if (Usuario.Apodo == UsuarioMemoria.Apodo)
            {
                IsModificarEnable = true;
                ApodoUsuario.Value = Usuario.Apodo;
                NombreUsuario.Value = Usuario.NombreP;
                ApellidosUsuario.Value = Usuario.ApellidoP;
                FotoPerfilUsuario.Value = Usuario.FotoPerfilP;
                EstadoUsuario.Value = Usuario.EstadoP;
                IsEliminarEnable = true;
                IsSeguirEnable = false;
                ((Command)UpdateUsuarioCommand).ChangeCanExecute();
                ((Command)DeleteUsuarioCommand).ChangeCanExecute();
            }
            else
            {
                IsModificarEnable = true;
            }
        }

        public async Task ActualizarUsuario()
        { 
            try
            {
                UsuarioModel usuario = new UsuarioModel()
                {
                    Apodo = ApodoUsuario.Value,
                    NombreP = NombreUsuario.Value,
                    ApellidoP = ApellidosUsuario.Value,
                    FotoPerfilP = FotoPerfilUsuario.Value,
                    EstadoP = EstadoUsuario.Value
                };
                APIResponse response = await UpdateUsuario.RunStrategy(usuario);
                if (response.IsSuccess)
                {
                    await loadDataHandler.PersistenceDataAsync("Usuario", usuario);
                    ((MessageViewModel)PopUp.BindingContext).Message = "Usuario actualizado exitosamente";
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
                    idUsuario = Usuario.idUsuario
                };
                APIResponse response = await DeleteUsuario.RunStrategy(usuario1);
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
                UsuarioModel UsuarioMemoria = (UsuarioModel)Application.Current.Properties["Usuario"];
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
            parametros.Parameters.Add(Usuario.idUsuario.ToString());
            APIResponse response = await GetSeguidores.RunStrategy(null, parametros);
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
                    idUsuario1 = usuario.idUsuario,
                    idUsuario2 = usuario.idUsuario,
                };
                APIResponse response = await DeleteSeguir.RunStrategy(peticion);
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
        

        #endregion Methods

    }
}
