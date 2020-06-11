using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using RedesSociales.ViewModels;

namespace RedesSociales.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarPerfilView : PopupPage
    {
        PerfilViewModel context = new PerfilViewModel();
        public EditarPerfilView()
        {
            InitializeComponent();
            BindingContext = context;
        }

        void Cerrar_Popup(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}