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

namespace RedesSociales.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        //Atributos
        private IGoogleClientManager googleClientManager;

        //Commands
        public ICommand InicioSesion { get; set; }

        //Getters y Setters
        public LoginViewModel()
        {
            InicioSesion = new Command(InicioSesionCommand);
            googleClientManager = CrossGoogleClient.Current;
        }

        private async void InicioSesionCommand()
        {
            googleClientManager.OnLogin += OnLoginCompletedAsync;
            try
            {
                await googleClientManager.LoginAsync();
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", e.ToString(), "OK");
            }
        }

        private async Task OnLoginCompletedAsync(object sender, GoogleClientResultEventArgs<GoogleUser> e)
        {
            if (e != null)
            {
                GoogleUser user = e.Data;
                UsuarioModel usuario=new UsuarioModel() {
                    apodo = user.Email,
                    Nombre = user.GivenName,
                    Apellidos = user.FamilyName,
                    FotoPerfil = user.Picture.ToString(),
                    Estado = "Activo"
                };
                await NavigationService.PushPage(new MainPage());
            }
        }
    }
}