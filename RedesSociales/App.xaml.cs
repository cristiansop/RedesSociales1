using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RedesSociales.Views;
using RedesSociales.Servicios.Navigation;
using RedesSociales.Servicios.Handler;
using RedesSociales.Models;

namespace RedesSociales
{
    public partial class App : Application
    {
        public bool IsLogin;
        #region Properties
        static NavigationService navigationService;
        static LoadDataHandler LoadData;
        #endregion

        #region Getters & Setters
        public static NavigationService NavigationService
        {
            get
            {
                if (navigationService == null)
                {
                    navigationService = new NavigationService();
                }
                return navigationService;
            }
        }
        #endregion Getters & Setters



        public App()
        {
            InitializeComponent();
            LoadData = new LoadDataHandler();
            CheckLogin();
            MainPage = new NavigationPage(new LoginView());
            if (IsLogin)
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                MainPage = new NavigationPage(new LoginView());
            }

        }
        public async void CheckLogin()
        {
            IsLogin = false;
            if(await LoadData.LoadData("Usuario.idUsuario"))
            {
                string idUsuarioT = (string)Current.Properties["Usuario.idUsuario"];
                IsLogin = true;
                await LoadData.LoadData("Usuario.Apodo");
                string Apodo = (string)Current.Properties["Usuario.Apodo"];
                await LoadData.LoadData("Usuario.Estado");
                string Estado = (string)Current.Properties["Usuario.Estado"];
                await LoadData.LoadData("Usuario.Nombre");
                string Nombre = (string)Current.Properties["Usuario.Nombre"];
                await LoadData.LoadData("Usuario.Apellido");
                string Apellido = (string)Current.Properties["Usuario.Apellido"];
                await LoadData.LoadData("Usuario.FotoPerfil");
                string FotoPerfil = (string)Current.Properties["Usuario.FotoPerfil"];
                UsuarioModel Usuario = new UsuarioModel()
                {
                    idUsuario = Int32.Parse(idUsuarioT),
                    Apodo = Apodo,
                    NombreP = Nombre,
                    ApellidoP = Apellido,
                    FotoPerfilP = FotoPerfil,
                    EstadoP = Estado
                };
                Current.Properties["Usuario"] = Usuario;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}