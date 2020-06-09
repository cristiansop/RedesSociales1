using Rg.Plugins.Popup.Pages;
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
    public partial class ComentsView : PopupPage
    {
        public ComentsView()
        {
            InitializeComponent();
        }

        void Cerrar_VistaComentarios(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}