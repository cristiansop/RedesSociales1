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
    public class UsuarioViewModel:NotificationObject
    {
        #region Properties

        #region Atributes

        public MessagePopupView PopUp { get; set; }
        private UsuarioModel usuario;
        private UsuarioModel usuario1;
        public ValidatableObject<string> ApodoUsuario { get; set; }
        public ValidatableObject<string> NombreUsuario { get; set; }
        public ValidatableObject<string> ApellidosUsuario { get; set; }
        public ValidatableObject<string> FotoPerfilUsuario { get; set; }
        public ValidatableObject<string> EstadoUsuario { get; set; }
        public ValidatableObject<string> BusquedaUsuario { get; set; }

        #region Enables
        #endregion Enables

        #endregion Atributes

        #region Request

        public ElegirRequest<UsuarioModel> CreateUsuario { get; set; }
        public ElegirRequest<UsuarioModel> GetUsuario { get; set; }
        public ElegirRequest<UsuarioModel> GetUsuario1 { get; set; }
        public ElegirRequest<UsuarioModel> UpdateUsuario { get; set; }
        public ElegirRequest<BaseModel> DeleteUsuario { get; set; }
        public ElegirRequest<BaseModel> CreateSeguir { get; set; }
        public ElegirRequest<BaseModel> GetSeguidos { get; set; }
        public ElegirRequest<BaseModel> GetSeguidos1 { get; set; }
        public ElegirRequest<BaseModel> GetSeguidores { get; set; }
        public ElegirRequest<BaseModel> GetSeguidores1 { get; set; }
        public ElegirRequest<BaseModel> DeleteSeguir { get; set; }

        #endregion Request

        #region Commands
        public ICommand CreateUsuarioCommand { get; set; }
        public ICommand GetUsuarioCommand { get; set; }
        public ICommand GetUsuario1Command { get; set; }
        public ICommand UpdateUsuarioCommand { get; set; }
        public ICommand DeleteUsuarioCommand { get; set; }
        public ICommand CreateSeguirCommand { get; set; }
        public ICommand GetSeguidosCommand { get; set; }
        public ICommand GetSeguidos1Command { get; set; }
        public ICommand GetSeguidoresCommand { get; set; }
        public ICommand GetSeguidores1Command { get; set; }
        public ICommand DeleteSeguirCommand { get; set; }

        #endregion Commands

        #endregion Properties

        #region Getters/Setters

        public UsuarioModel Usuario
        {
            get { return usuario; }
            set { usuario = value; OnPropertyChanged(); }
        }
        public UsuarioModel Usuario1
        {
            get { return usuario1; }
            set { usuario1 = value; OnPropertyChanged(); }
        }

        #endregion Getters/Setters

        #region Initialize
        public UsuarioViewModel()
        {
            PopUp = new MessagePopupView();
            Usuario = new UsuarioModel();

            Usuario.Idusuario = 1; 
            Usuario.apodo = "el_gusano01"; 
            Usuario.Nombre = "Nicolas"; 
            Usuario.Apellidos = "El Golozo"; 
            Usuario.Estado = "activao";

            Usuario1 = new UsuarioModel();
            InitializeRequest();
            InitializeCommands();
            InitializeFields();
        }
        public void InitializeRequest()
        {
            #region Url
            string urlCretateUsuario = Endpoints.URL_SERVIDOR + Endpoints.CREATE_USUARIO;
            string urlGetUsuario = Endpoints.URL_SERVIDOR + Endpoints.GET_USUARIO;
            string urlUpdateUsuario = Endpoints.URL_SERVIDOR + Endpoints.UPDATE_USUARIO;
            string urlDeleteUsuario = Endpoints.URL_SERVIDOR + Endpoints.DELETE_USUARIO;
            string urlCreateSeguir = Endpoints.URL_SERVIDOR + Endpoints.CREATE_SEGUIR;
            string urlGetSeguidos = Endpoints.URL_SERVIDOR + Endpoints.GET_SEGUIDOS;
            string urlGetSeguidores = Endpoints.URL_SERVIDOR + Endpoints.GET_SEGUIDORES;
            string urlDeleteSeguir = Endpoints.URL_SERVIDOR + Endpoints.DELETE_SEGUIR;
            #endregion Url

            #region API
            CreateUsuario = new ElegirRequest<UsuarioModel>();
            CreateUsuario.ElegirEstrategia("POST", urlCretateUsuario);

            GetUsuario = new ElegirRequest<UsuarioModel>();
            GetUsuario.ElegirEstrategia("GET", urlGetUsuario);

            GetUsuario1 = new ElegirRequest<UsuarioModel>();
            GetUsuario1.ElegirEstrategia("GET", urlGetUsuario);

            UpdateUsuario = new ElegirRequest<UsuarioModel>();
            UpdateUsuario.ElegirEstrategia("POST", urlUpdateUsuario);

            DeleteUsuario = new ElegirRequest<BaseModel>();
            DeleteUsuario.ElegirEstrategia("POST", urlDeleteUsuario);

            CreateSeguir = new ElegirRequest<BaseModel>();
            CreateSeguir.ElegirEstrategia("POST", urlCreateSeguir);

            GetSeguidos = new ElegirRequest<BaseModel>();
            GetSeguidos.ElegirEstrategia("GET", urlGetSeguidos);

            GetSeguidos1 = new ElegirRequest<BaseModel>();
            GetSeguidos1.ElegirEstrategia("GET", urlGetSeguidos);

            GetSeguidores = new ElegirRequest<BaseModel>();
            GetSeguidores.ElegirEstrategia("GET", urlGetSeguidores);

            GetSeguidores1 = new ElegirRequest<BaseModel>();
            GetSeguidores1.ElegirEstrategia("GET", urlGetSeguidores);

            DeleteSeguir = new ElegirRequest<BaseModel>();
            DeleteSeguir.ElegirEstrategia("POST", urlDeleteSeguir);
            #endregion API
        }
        public void InitializeCommands()
        {
            #region Comandos

            CreateUsuarioCommand = new Command(async () => await CrearUsuario(), () => true);
            GetUsuarioCommand = new Command(async () => await SeleccionarUsuario(), () => true);
            GetUsuario1Command = new Command(async () => await SeleccionarUsuario1(), () => true);
            UpdateUsuarioCommand = new Command(async () => await ActualizarUsuario(), () => true);
            DeleteUsuarioCommand = new Command(async () => await EliminarUsuario(), () => true);
            CreateSeguirCommand = new Command(async () => await CrearSeguir(), () => true);
            GetSeguidosCommand = new Command(async () => await SeleccionarSeguidos(), () => true);
            GetSeguidos1Command = new Command(async () => await SeleccionarSeguidos1(), () => true);
            GetSeguidoresCommand = new Command(async () => await SeleccionarSeguidores(), () => true);
            GetSeguidores1Command = new Command(async () => await SeleccionarSeguidores1(), () => true);
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
            BusquedaUsuario= new ValidatableObject<string>();

            ApodoUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El apodo del usuario es obligatorio" });
            NombreUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El nombre del usuario es obligatorio" });
            ApellidosUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "los apellidos del usuario es obligatorio" });
            EstadoUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El Estado de la cuenta es obligatorio" });
            BusquedaUsuario.Validations.Add(new RequiredRule<string> { ValidationMessage = "El apodo del usuario es obligatorio" });
        }
        #endregion Initialize

        #region Methods

        public async Task CrearUsuario()
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
                APIResponse response = await CreateUsuario.EjecutarEstrategia(usuario);
                if (response.isSuccess)
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Usuario creado exitosamente";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
                else
                {
                    ((MessageViewModel)PopUp.BindingContext).Message = "Error al crear usuario";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {

            }
        }

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
                if (response.isSuccess)
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
                if (response.isSuccess)
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
            throw new NotImplementedException();
        }

        public async Task SeleccionarSeguidos()
        {
            ParametersRequest parametros = new ParametersRequest();
            parametros.Parametros.Add(Usuario.Idusuario.ToString());
            APIResponse response = await GetSeguidos.EjecutarEstrategia(null, parametros);
            if (response.isSuccess)
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

        private async Task SeleccionarSeguidos1()
        {
            ParametersRequest parametros = new ParametersRequest();
            parametros.Parametros.Add(Usuario1.Idusuario.ToString());
            APIResponse response = await GetSeguidos.EjecutarEstrategia(null, parametros);
            if (response.isSuccess)
            {
                List<UsuarioModel> usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(response.Response);
                Usuario1.Seguidos = usuarios;

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
            if (response.isSuccess)
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

        private async Task SeleccionarSeguidores1()
        {
            ParametersRequest parametros = new ParametersRequest();
            parametros.Parametros.Add(Usuario1.Idusuario.ToString());
            APIResponse response = await GetSeguidores.EjecutarEstrategia(null, parametros);
            if (response.isSuccess)
            {
                List<UsuarioModel> usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(response.Response);
                Usuario1.Seguidores = usuarios;

            }
            else
            {
                ((MessageViewModel)PopUp.BindingContext).Message = "Error encontrar los seguidores del usuario";
                await PopupNavigation.Instance.PushAsync(PopUp);
            }
        }

        public async Task EliminarSeguir()
        {
            throw new NotImplementedException();
        }

        public async Task SeleccionarUsuario()
        {
            try
            {
                ParametersRequest parametros = new ParametersRequest();
                parametros.Parametros.Add(BusquedaUsuario.Value);
                APIResponse response = await GetUsuario.EjecutarEstrategia(null, parametros);
                if (response.isSuccess)
                {
                    Usuario = JsonConvert.DeserializeObject<UsuarioModel>(response.Response);
                    //setear los entrys o lo que sea con los datos ej:NombreUsuario.Value = Usuario.Nombre;
                    //IsEliminarEnable = true;
                    //IsGuardarEnable = true;
                    //IsGuardarEditar = true;
                    //((Command)CrearCategoriaCommand).ChangeCanExecute();
                    //((Command)EliminarCategoriaCommand).ChangeCanExecute();
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

        private async Task SeleccionarUsuario1()
        {
            try
            {
                ParametersRequest parametros = new ParametersRequest();
                parametros.Parametros.Add(BusquedaUsuario.Value);
                APIResponse response = await GetUsuario.EjecutarEstrategia(null, parametros);
                if (response.isSuccess)
                {
                    Usuario1 = JsonConvert.DeserializeObject<UsuarioModel>(response.Response);
                    //setear los entrys o lo que sea con los datos ej:NombreUsuario.Value = Usuario.Nombre;
                    //IsEliminarEnable = true;
                    //IsGuardarEnable = true;
                    //IsGuardarEditar = true;
                    //((Command)CrearCategoriaCommand).ChangeCanExecute();
                    //((Command)EliminarCategoriaCommand).ChangeCanExecute();
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

        #endregion Methods

    }
}
