using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using RedesSociales.ViewModels;

namespace RedesSociales.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagePopupView : PopupPage
    {
        MessageViewModel context = new MessageViewModel();
        public MessagePopupView()
        {
            InitializeComponent();
            BindingContext = context;
        }
    }
}