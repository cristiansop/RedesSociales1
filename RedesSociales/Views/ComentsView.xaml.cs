using RedesSociales.Models;
using RedesSociales.ViewModels;
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
    public partial class ComentsView : ContentPage
    {
        ComentarioViewModel context = new ComentarioViewModel();
        public ComentsView()
        {
            InitializeComponent();
            BindingContext = context;
        }

        /*
        void Cerrar_VistaComentarios(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }*/
    }
}