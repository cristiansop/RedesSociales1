using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using RedesSociales.Servicios.Handler;
using RedesSociales.Configuracion;
using RedesSociales.Models;
using RedesSociales.Models.Auxiliary;
using RedesSociales.Servicios.Rest;
using RedesSociales.Servicios.Propagacion;
using RedesSociales.Validations.Base;
using RedesSociales.Validations.Rules;
using RedesSociales.Views;
namespace RedesSociales.ViewModels
{ 
    public class PerfilViewModel : ViewModelBase
    {
        #region Properties

        #region Atributes
        private LoadDataHandler loadDataHandler;

        public MessagePopupView PopUp { get; set; }

        private UsuarioModel usuario;

        public ValidatableObject<string> ApodoUsuario { get; set; }

        public ValidatableObject<string> NombreUsuario { get; set; }

        public ValidatableObject<string> ApellidosUsuario { get; set; }

        public ValidatableObject<string> FotoPerfilUsuario { get; set; }

        public ValidatableObject<string> EstadoUsuario { get; set; }

        public string  NombreInicial { get; set; }
        public string  ApellidoInicial { get; set; }
        public string EstadoInicial { get; set; }

        private bool isEditEnable;
        private int seguidores;
        private int seguidos;
        #endregion properties

        #region Sets/Gets
        public bool IsEditEnable
        {
            get { return isEditEnable; }
            set
            {
                isEditEnable = value;
                OnPropertyChanged();
            }
        }
        public int Seguidores
        {
            get { return seguidores; }
            set
            {
                seguidores = value;
                OnPropertyChanged();
            }
        }
        public int Seguidos
        {
            get { return seguidos; }
            set
            {
                seguidos = value;
                OnPropertyChanged();
            }
        }

        #endregion Atributes

        #region Request
        public SelectRequest<UsuarioModel> UpdateUsuario { get; set; }
        public SelectRequest<BaseModel> DeleteUsuario { get; set; }
        public SelectRequest<BaseModel> GetSeguidos { get; set; }
        public SelectRequest<BaseModel> GetSeguidores { get; set; }
        public SelectRequest<PeticionesDosUsuariosModel> DeleteSeguir { get; set; }
        public SelectRequest<BaseModel> GetPublicacionesUsuario { get; set; }
        #endregion Request

        #region Commands
        public ICommand UpdateUsuarioCommand { get; set; }
        public ICommand DeleteUsuarioCommand { get; set; }
        public ICommand GetSeguidosCommand { get; set; }
        public ICommand GetSeguidoresCommand { get; set; }
        public ICommand DeleteSeguirCommand { get; set; }
        public ICommand GetPublicacionesUsuarioCommand { get; set; }
        public ICommand ValidateFormCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        #endregion Commands

        #endregion Properties

        #region Getters/Setters

        public UsuarioModel Usuario
        {
            get { return usuario; }
            set
            {
                usuario = value;
                OnPropertyChanged();
            }
        }
        #endregion Getters/Setters

        #region Initialize
        public PerfilViewModel()
        {
            loadDataHandler = new LoadDataHandler();
            PopUp = new MessagePopupView();
            Usuario = (UsuarioModel)Application.Current.Properties["Usuario"];
            IsEditEnable = false;
            InitializeRequest();
            InitializeCommands();
            InitializeFields();
            ActualizarPerfil();
            AddValidations();
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
            string urlGetPublicacionesUsuario = Endpoints.URL_SERVIDOR + Endpoints.GET_PUBLICACIONES_USUARIO;
            #endregion Url

            #region API

            UpdateUsuario = new SelectRequest<UsuarioModel>();
            UpdateUsuario.SelectStrategy("POST", urlUpdateUsuario);

            DeleteUsuario = new SelectRequest<BaseModel>();
            DeleteUsuario.SelectStrategy("POST", urlDeleteUsuario);

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

            UpdateUsuarioCommand = new Command(async () => await ActualizarUsuario(), () => IsEditEnable);
            DeleteUsuarioCommand = new Command(async () => await EliminarUsuario(), () => true);
            GetSeguidosCommand = new Command(async () => await SeleccionarSeguidos(), () => true);
            GetSeguidoresCommand = new Command(async () => await SeleccionarSeguidores(), () => true);
            ValidateFormCommand = new Command(() => ValidateForm());
            GetPublicacionesUsuarioCommand = new Command(async () => await SeleccionarPublicacionesUsuario(), () => true);
            RefreshCommand = new Command(() => ActualizarPerfil(), () => true);

            #endregion Comandos
        }

        public void InitializeFields()
        {
            ApodoUsuario = new ValidatableObject<string>();
            ApodoUsuario.Value = Usuario.Apodo;
            NombreUsuario = new ValidatableObject<string>();
            ApellidosUsuario = new ValidatableObject<string>();
            FotoPerfilUsuario = new ValidatableObject<string>();
            EstadoUsuario = new ValidatableObject<string>();
            Seguidores = Usuario.Seguidores.Count;
            Seguidos = Usuario.Seguidos.Count;
            EstadoUsuario.Value = Usuario.EstadoP;
            NombreInicial = Usuario.NombreP;
            ApellidoInicial = Usuario.ApellidoP;
            EstadoInicial = Usuario.EstadoP;
        }
        public void AddValidations()
        {
            ApodoUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El Apodo del usuario es obligatorio" });
            NombreUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El nombre del usuario es obligatorio" });
            ApellidosUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "los apellidos del usuario es obligatorio" });
            EstadoUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El Estado de la cuenta es obligatorio" });
        }
        #endregion Initialize

        #region Methods
        private void ValidateForm()
        {
            bool NombreUsuarioValidate = NombreUsuario.Validate();
            bool ApellidosUsuarioValidate = ApellidosUsuario.Validate();
            bool EstadoUsuarioValidate = EstadoUsuario.Validate();
            IsEditEnable = NombreUsuarioValidate && ApellidosUsuarioValidate && EstadoUsuarioValidate;
            ((Command)UpdateUsuarioCommand).ChangeCanExecute();
        }

        public async void ActualizarPerfil()
        {
            await SeleccionarPublicacionesUsuario();
            await SeleccionarSeguidores();
            await SeleccionarSeguidos();

        }
        public async Task ActualizarUsuario()
        {
            try
            {
                UsuarioModel usuario = new UsuarioModel()
                {
                    idUsuario = Usuario.idUsuario,
                    Apodo = ApodoUsuario.Value,
                    NombreP = NombreUsuario.Value,
                    ApellidoP = ApellidosUsuario.Value,
                    FotoPerfilP = "",
                    EstadoP = EstadoUsuario.Value
                };
                APIResponse response = await UpdateUsuario.RunStrategy(usuario);
                if (response.IsSuccess)
                {
                    await loadDataHandler.PersistenceDataAsync("Usuario", usuario);
                    Usuario.FotoPerfilP = usuario.FotoPerfilP;
                    Usuario.NombreP = usuario.NombreP;
                    Usuario.ApellidoP = usuario.ApellidoP;
                    Usuario.EstadoP = usuario.EstadoP;
                    ((MessageViewModel)PopUp.BindingContext).Message = "Usuario actualizado exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    await PopupNavigation.Instance.PopAsync();
                    await PopupNavigation.Instance.PopAsync();
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al actualizar usuario";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    await PopupNavigation.Instance.PopAsync();
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
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al eliminar usuario";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    await PopupNavigation.Instance.PopAsync();
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
                    Seguidos = Usuario.Seguidos.Count;
                }
            }
            else
            {
                ((MessageViewModel)PopUp.BindingContext).Message = "Error encontrar los seguidos del usuario";
                await PopupNavigation.Instance.PushAsync(PopUp);
                await Task.Delay(TimeSpan.FromSeconds(1));
                await PopupNavigation.Instance.PopAsync();
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
                    Seguidores = Usuario.Seguidores.Count;
                }
                

            }
            else
            {
                ((MessageViewModel)PopUp.BindingContext).Message = "Error encontrar los seguidores del usuario";
                await PopupNavigation.Instance.PushAsync(PopUp);
                await Task.Delay(TimeSpan.FromSeconds(1));
                await PopupNavigation.Instance.PopAsync();
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
        #endregion Methods

    }
}
