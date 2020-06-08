using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RedesSociales.ViewModels;

namespace RedesSociales.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        LoginViewModel context = new LoginViewModel();
        public LoginView()
        {
            InitializeComponent();
            BindingContext = context;
            btn_google.Clicked += Buttom_IniciarSesion_Clicked;
        }

        public void Buttom_IniciarSesion_Clicked(object sender, EventArgs e)
        {
            ((NavigationPage)this.Parent).PushAsync(new MainPage());
        }
    }
}