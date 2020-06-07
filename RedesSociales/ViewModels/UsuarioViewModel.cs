using RedesSociales.Configuracion;
using RedesSociales.Models;
using RedesSociales.Models.AuxiliarModels;
using RedesSociales.Servicios.APIRest;
using RedesSociales.Servicios.Propagacion;
using RedesSociales.Validations.Base;
using RedesSociales.Validations.Rules;
using RedesSociales.Views;
using Newtonsoft.Json;
//using Rg.Plugins.Popup.Services;
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
        //public MessageViewPop PopUp { get; set; }

        public ValidatableObject<string> ApodoUsuario { get; set; }
        public ValidatableObject<string> NombreUsuario { get; set; }
        public ValidatableObject<string> ApellidosUsuario { get; set; }
        public ValidatableObject<string> FotoPerfilUsuario { get; set; }
        public ValidatableObject<string> EstadoUsuario { get; set; }
    
        #region Enables
        #endregion Enables
        #endregion Atributes
        #region Request
        public ElegirRequest<UsuarioModel> CreateUsuario { get; set; }
        public ElegirRequest<UsuarioModel> GetUsuario { get; set; }
        public ElegirRequest<UsuarioModel> UpdateUsuario { get; set; }
        public ElegirRequest<BaseModel> DeleteUsuario { get; set; }
        public ElegirRequest<BaseModel> CreateSeguir { get; set; }
        public ElegirRequest<BaseModel> GetSeguidos { get; set; }
        public ElegirRequest<BaseModel> GetSeguidores { get; set; }
        public ElegirRequest<BaseModel> DeleteSeguir { get; set; }
        #endregion Request
        #region Commands
        public ICommand CreateUsuarioCommand { get; set; }
        public ICommand GetUsuarioCommand { get; set; }
        public ICommand UpdateUsuarioCommand { get; set; }
        public ICommand DeleteUsuarioCommand { get; set; }
        public ICommand CreateSeguirCommand { get; set; }
        public ICommand GetSeguidosCommand { get; set; }
        public ICommand GetSeguidoresCommand { get; set; }
        public ICommand DeleteSeguirCommand { get; set; }
        #endregion Commands
        #endregion Properties
        #region Getters/Setters
        #endregion Getters/Setters
        #region Initialize
        public UsuarioViewModel()
        {
            //PopUp = new MessageViewPop();
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

            UpdateUsuario = new ElegirRequest<UsuarioModel>();
            UpdateUsuario.ElegirEstrategia("POST", urlUpdateUsuario);

            DeleteUsuario = new ElegirRequest<BaseModel>();
            DeleteUsuario.ElegirEstrategia("POST", urlDeleteUsuario);

            CreateSeguir = new ElegirRequest<BaseModel>();
            CreateSeguir.ElegirEstrategia("POST", urlCreateSeguir);

            GetSeguidos = new ElegirRequest<BaseModel>();
            GetSeguidos.ElegirEstrategia("GET", urlGetSeguidos);

            GetSeguidores = new ElegirRequest<BaseModel>();
            GetSeguidores.ElegirEstrategia("GET", urlGetSeguidores);

            DeleteSeguir = new ElegirRequest<BaseModel>();
            DeleteSeguir.ElegirEstrategia("POST", urlDeleteSeguir);
            #endregion API
        }
        public void InitializeCommands()
        {
            #region Comandos
            CreateUsuarioCommand = new Command(async () => await CrearUsuario(), () => true);
            GetUsuarioCommand = new Command(async () => await SeleccionarUsuario(), () => true);
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


        public async Task CrearUsuario()
        {/*
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

            }*/
        }

        public async Task ActualizarUsuario()
        {/*
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

            }*/
        }

        public async Task EliminarUsuario()
        {
            throw new NotImplementedException();
        }

        public async Task CrearSeguir()
        {
            throw new NotImplementedException();
        }

        public async Task SeleccionarSeguidos()
        {
            ParametersRequest parametros = new ParametersRequest();
            parametros.Parametros.Add("1");
            APIResponse respose = await GetSeguidos.EjecutarEstrategia(null, parametros);
            List<UsuarioModel> usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(respose.Response);
        }

        public async Task SeleccionarSeguidores()
        {
            ParametersRequest parametros = new ParametersRequest();
            parametros.Parametros.Add("1");
            APIResponse respose = await GetSeguidores.EjecutarEstrategia(null, parametros);
            List<UsuarioModel> usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(respose.Response);
        }

        public async Task EliminarSeguir()
        {
            throw new NotImplementedException();
        }

        public async Task SeleccionarUsuario()
        {
            ParametersRequest parametros = new ParametersRequest();
            parametros.Parametros.Add("jhoan1");
            APIResponse respose = await GetUsuario.EjecutarEstrategia(null, parametros);
            UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(respose.Response);
        }

    }
}
