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
using RedesSociales.Models.Auxiliary;
using RedesSociales.Servicios.Rest;
using RedesSociales.Views;
using RedesSociales.Servicios.Handler;
using RedesSociales.Configuracion;
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
        public SelectRequest<UsuarioModel> CreateUsuario { get; set; }
        public SelectRequest<UsuarioModel> GetUsuario { get; set; }
        public async Task CrearUsuario()
        {
            try
            {
                UsuarioModel usuario = new UsuarioModel()
                {
                    idUsuario = 0,
                    Apodo = Usuario.Apodo,
                    NombreP = Usuario.NombreP,
                    ApellidoP = Usuario.ApellidoP,
                    FotoPerfilP = Usuario.FotoPerfilP,
                    EstadoP = Usuario.EstadoP
                };
                APIResponse response = await CreateUsuario.RunStrategy(Usuario);
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
                parametros.Parameters.Add("u1");
                APIResponse response = await GetUsuario.RunStrategy(null, parametros);
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


            CreateUsuario = new SelectRequest<UsuarioModel>();
            CreateUsuario.SelectStrategy("POST", urlCretateUsuario);
            GetUsuario = new SelectRequest<UsuarioModel>();
            GetUsuario.SelectStrategy("GET", urlGetUsuario);
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
                        idUsuario = 0,
                        Apodo = user.Email,
                        NombreP = user.GivenName,
                        ApellidoP = user.FamilyName,
                        FotoPerfilP = user.Picture.ToString(),
                        EstadoP = "Activo"
                    };
                    //await CrearUsuario();
                    //await SeleccionarUsuario();
                    //Console.WriteLine(Usuario.NombreP);

                    //SelectRequest<UsuarioModel> request = new SelectRequest<UsuarioModel>();
                    //request.SelectStrategy("GET", "http://192.168.1.2:9000/usuario/get/u1");
                    //request.SelectStrategy("GET", "https://f83c42a2-6f28-4213-acde-43515fdba286.mock.pstmn.io/usuario/get");
                    //APIResponse response = await request.RunStrategy(null);
                   // Usuario = JsonConvert.DeserializeObject<UsuarioModel>(response.Response);
                    //Console.WriteLine(response);


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