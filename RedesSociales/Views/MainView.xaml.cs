using RedesSociales.Models;
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
    public partial class MainView : ContentPage
    {
        PublicacionViewModel context = new PublicacionViewModel();
        public MainView()
        {
            InitializeComponent();
            BindingContext = context;
        }

        private async void Buttton_Comentarios(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new EditarPerfilView());
        }

        private void Seleccionar_Publicacion(object sender, SelectionChangedEventArgs e)
        {
            var publicacion = e.CurrentSelection.FirstOrDefault() as PublicacionModel;

            if (publicacion == null)
                return;

            //Navigation.PushAsync(new PerfilView());
            context.TraerPublicacionDetalle(publicacion);

            ((CollectionView)sender).SelectedItem = null;
        }

    }
}