using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using RedesSociales.Configuracion;
using RedesSociales.Models;
using RedesSociales.Models.Auxiliary;
using RedesSociales.Servicios.Rest;
using RedesSociales.Validations.Base;
using RedesSociales.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RedesSociales.ViewModels
{
    public class NuevaPublicacionViewModel : ViewModelBase
    {
        #region Properties
        #region Atributes
        public UsuarioModel Usuario { get; set; } 
        public ValidatableObject<string> TipoPublicacion { get; set; }
        public ValidatableObject<string> DescripcionPublicacion { get; set; }
        public ICommand SeleccionarFoto { get; set; }
        public ICommand Enviar { get; set; }
        private ImageSource imagen { get; set; }
        private MemoryStream memoryStream { get; set; }

        public SelectRequest<PublicacionModel> CreatePublicacionRequest;
        public MessagePopupView PopUp { get; set; }

        public bool IsEnableSend;

        private string descripcion { get; set; }
        public string Descripcion
        {
            get { return descripcion; }
            set
            {
                descripcion = value;
                OnPropertyChanged();
            }
        }

        private string stringPicture;
        public string StringPicture
        {
            get { return stringPicture; }
            set
            {
                stringPicture = value;
                OnPropertyChanged();
            }
        }


        #endregion Atributes
        #endregion Properties

        public NuevaPublicacionViewModel()
        {
            Usuario = (UsuarioModel)Application.Current.Properties["Usuario"];
            IsEnableSend = false;
            SeleccionarFoto = new Command(async () => await SeleccionarFotoCommand(), () => true);
            Enviar = new Command(async () => await EnviarCommand(), () => IsEnableSend);
            memoryStream = new MemoryStream();

            string urlCreatePublicacion = Endpoints.URL_SERVIDOR + Endpoints.CREATE_PUBLICACION;
            CreatePublicacionRequest = new SelectRequest<PublicacionModel>();
            CreatePublicacionRequest.SelectStrategy("POST", urlCreatePublicacion);
            PopUp = new MessagePopupView();
        }

        #region rest
        public async Task<bool> CreatePublicacion()
        {
            try
            {
                PublicacionModel publicacion = new PublicacionModel()
                {
                    idUsuario = Usuario.idUsuario,
                    Apodo = Usuario.Apodo,
                    idPublicacion = 0,
                    Archivo = StringPicture,
                    Tipo = 0,
                    Descripcion = Descripcion,
                    Tiempo = ""
                };
                APIResponse response = await CreatePublicacionRequest.RunStrategy(publicacion);
                if (response.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                string s = e.Message;
                Console.WriteLine(s);
                return false;
            }
        }
        #endregion rest

        public ImageSource Imagen
        {
            get { return imagen; }
            set { imagen = value; OnPropertyChanged(); }
        }

        private async Task SeleccionarFotoCommand()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                return;
            }

            MediaFile file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.MaxWidthHeight,
            });

            if (file == null)
                return;

            Imagen = ImageSource.FromStream(() =>
            {
                Stream stream = file.GetStream();
                file.GetStream().CopyTo(memoryStream);
                file.Dispose();
                return stream;
            });
            IsEnableSend = true;
            ((Command)Enviar).ChangeCanExecute();
        }

        private async Task EnviarCommand()
        {
            string base64String = Convert.ToBase64String(memoryStream.ToArray());
            StringPicture = base64String;
            memoryStream = new MemoryStream();
            if (await CreatePublicacion())
            {
                ((MessageViewModel)PopUp.BindingContext).Message = "Publicacion creada exitosamente";
                await PopupNavigation.Instance.PushAsync(PopUp);
                await Task.Delay(TimeSpan.FromSeconds(1.5));
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                ((MessageViewModel)PopUp.BindingContext).Message = "Error al crear publicacion";
                await PopupNavigation.Instance.PushAsync(PopUp);
                await Task.Delay(TimeSpan.FromSeconds(1.5));
                await PopupNavigation.Instance.PopAsync();
            }
            Imagen = null;
            Descripcion = "";
            IsEnableSend = false;
            ((Command)Enviar).ChangeCanExecute();
        }
    }
}
