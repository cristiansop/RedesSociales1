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
using RedesSociales.Servicios.APIRest;
using RedesSociales.Views;
using RedesSociales.Servicios.Handler;
using RedesSociales.Configuracion;
using Newtonsoft.Json;
using RedesSociales.Models.AuxiliarModels;

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
                    Idusuario=0,
                    apodo = Usuario.apodo,
                    Nombre = Usuario.Nombre,
                    Apellidos = Usuario.Apellidos,
                    FotoPerfil = Usuario.FotoPerfil,
                    Estado = Usuario.Estado
                };
                APIResponse response = await CreateUsuario.EjecutarEstrategia(Usuario);
                if (response.IsSuccess)
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
                string s = e.Message;
                Console.WriteLine(s);
                Console.WriteLine(s);
            }
        }
        public async Task SeleccionarUsuario()
        {
            try
            {
                ParametersRequest parametros = new ParametersRequest();
                parametros.Parametros.Add("Nicortiz738273822");
                APIResponse response = await GetUsuario.EjecutarEstrategia(null, parametros);
                if (response.IsSuccess)
                {
                    Usuario = JsonConvert.DeserializeObject<UsuarioModel>(response.Response);
                    //agregar cambios necesarios para jhoan que aun no dice
                }
                else
                {

                    ((MessageViewModel)PopUp.BindingContext).Message = "No se encuentra el usuario";
                    await PopupNavigation.Instance.PushAsync(PopUp);
                }
            }
            catch (Exception e)
            {
                string s = e.Message;
                Console.WriteLine(s);
                Console.WriteLine(s);

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
                        apodo = user.Email,
                        Nombre = user.GivenName,
                        Apellidos = user.FamilyName,
                        FotoPerfil = user.Picture.ToString(),
                        Estado = "Activo"
                    };
                    await CrearUsuario();
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