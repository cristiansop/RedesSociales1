using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using RedesSociales.Servicios.Propagacion;
namespace RedesSociales.ViewModels
{
    public class LoginViewModel : NotificationObject
    {
        //Atributos
        private string nombre { get; set; }
        private IGoogleClientManager googleClientManager;

        //Commands
        public ICommand InicioSesion { get; set; }


        //Getters y Setters
        public string NombrePersona
        {
            get { return nombre; }
            set
            {
                nombre = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel()
        {
            InicioSesion = new Command(InicioSesionCommand);
            googleClientManager = CrossGoogleClient.Current;
        }

        private async void InicioSesionCommand()
        {
            googleClientManager.OnLogin += OnLoginCompleted;
            try
            {
                await googleClientManager.LoginAsync();
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", e.ToString(), "OK");
            }

        }

        private void OnLoginCompleted(object sender, GoogleClientResultEventArgs<GoogleUser> e)
        {
            if (e != null)
            {
                GoogleUser user = e.Data;
                NombrePersona = user.Email;
            }
        }
    }
}