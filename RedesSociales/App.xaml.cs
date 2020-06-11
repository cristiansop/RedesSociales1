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
            // -- test 
            Test();
            // --
            MainPage = new NavigationPage(new MainPage());
        }

        /// <summary>
        public async void Test()
        {
            UsuarioModel Usuario = new UsuarioModel()
            {
                idUsuario = 0,
                Apodo = "jhoanseb",
                NombreP = "Jhoan",
                ApellidoP = "Lozano",
                FotoPerfilP = "",
                EstadoP = "Activo"
            };
            await LoadData.PersistenceDataAsync("Usuario", Usuario);
        }
        /// </summary>

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