using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using RedesSociales.Servicios.Propagacion;
using Rg.Plugins.Popup.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using RedesSociales.Models;
using RedesSociales.Servicios.Navigation;
using RedesSociales.Servicios.Handler;
using RedesSociales.Configuracion;
using RedesSociales.Models.AuxiliarModels;
using RedesSociales.Servicios.APIRest;
using RedesSociales.Views;
using Newtonsoft.Json;

namespace RedesSociales.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        //Atributos
        private IGoogleClientManager googleClientManager;

        //Commands
        public ICommand InicioSesion { get; set; }

        private LoadDataHandler loadDataHandler;
        public MessagePopupView PopUp { get; set; }
        public UsuarioModel Usuario { get; set; }


        //Getters y Setters
        public ElegirRequest<UsuarioModel> CreateUsuario { get; set; }
        public ElegirRequest<UsuarioModel> GetUsuario { get; set; }

        public async Task CrearUsuario()
        {
            try
            {
                UsuarioModel usuario = new UsuarioModel()
                {
                    apodo = Usuario.apodo,
                    Nombre = Usuario.Nombre,
                    Apellidos = Usuario.Apellidos,
                    FotoPerfil = Usuario.FotoPerfil,
                    Estado = Usuario.Estado
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
        public async Task SeleccionarUsuario()
        {
            try
            {
                ParametersRequest parametros = new ParametersRequest();
                parametros.Parametros.Add(Usuario.apodo);
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


        public LoginViewModel()
        {
            InicioSesion = new Command(InicioSesionCommand);
            googleClientManager = CrossGoogleClient.Current;
            loadDataHandler = new LoadDataHandler();
            string urlCretateUsuario = Endpoints.URL_SERVIDOR + Endpoints.CREATE_USUARIO;
            string urlGetUsuario = Endpoints.URL_SERVIDOR + Endpoints.GET_USUARIO;


            CreateUsuario = new ElegirRequest<UsuarioModel>();
            CreateUsuario.ElegirEstrategia("POST", urlCretateUsuario);
            GetUsuario = new ElegirRequest<UsuarioModel>();
            GetUsuario.ElegirEstrategia("GET", urlGetUsuario);
            PopUp = new MessagePopupView();
        }

        private async void InicioSesionCommand()
        {
            EventHandler<GoogleClientResultEventArgs<GoogleUser>> userLoginDelegate = null;
            userLoginDelegate = async (object sender, GoogleClientResultEventArgs<GoogleUser> e) =>
            {
                if (e != null)
                {
                    GoogleUser user = e.Data;
                    Usuario = new UsuarioModel()
                    {
                        apodo = "u1",
                        Nombre = user.GivenName,
                        Apellidos = user.FamilyName,
                        FotoPerfil = user.Picture.ToString(),
                        Estado = "Activo"
                    };
                    await SeleccionarUsuario();
                    await loadDataHandler.PersistenceDataAsync("Usuario", Usuario);
                    await NavigationService.PushPage(new MainPage());
                }
            };
            googleClientManager.OnLogin += userLoginDelegate;
            try
            {
                await googleClientManager.LoginAsync();
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", e.ToString(), "OK");
            }
        }
    }
}