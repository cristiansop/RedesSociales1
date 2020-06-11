using RedesSociales.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RedesSociales.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MiPerfilView : ContentPage
    {
        PerfilViewModel context = new PerfilViewModel();

        public MiPerfilView()
        {
            InitializeComponent();
            BindingContext = context;
        }
        private async void Buttton_EditarPerfil(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new EditarPerfilView());
        }
    }
}