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
using System.Collections.Generic;
using System.IO;

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
                parametros.Parameters.Add(Usuario.Apodo);
                APIResponse response = await GetUsuario.RunStrategy(null, parametros);
                if (response.IsSuccess)
                {
                    UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(response.Response);
                    if (usuario.Apodo==Usuario.Apodo)
                    {
                        Usuario = usuario;
                    }
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
                        Apodo = user.Email.Remove(user.Email.LastIndexOf('@')),
                        NombreP = user.GivenName,
                        ApellidoP = user.FamilyName,
                        FotoPerfilP = "",
                        EstadoP = "Activo"
                    };
                    await SeleccionarUsuario();
                    if (Usuario.idUsuario == 0)
                    {
                        await CrearUsuario();
                        await SeleccionarUsuario();
                    }
                    if(Usuario.idUsuario != 0)
                    {
                        StorageUser(Usuario);
                        await loadDataHandler.PersistenceDataAsync("Usuario", Usuario);
                        Application.Current.MainPage = new MainPage();
                    }
                    else
                    {
                        ((MessageViewModel)PopUp.BindingContext).Message = "Error al conectar con el servidor";
                        await PopupNavigation.Instance.PushAsync(PopUp);

                    }
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
        public async void StorageUser(UsuarioModel user)
        {
            await loadDataHandler.PersistenceDataAsync("Usuario.idUsuario", user.idUsuario);
            await loadDataHandler.PersistenceDataAsync("Usuario.Apodo", user.Apodo);
            await loadDataHandler.PersistenceDataAsync("Usuario.Estado", user.EstadoP);
            await loadDataHandler.PersistenceDataAsync("Usuario.Nombre", user.NombreP);
            await loadDataHandler.PersistenceDataAsync("Usuario.Apellido", user.ApellidoP);
            await loadDataHandler.PersistenceDataAsync("Usuario.FotoPerfil", user.FotoPerfilP);
        }
    }
}