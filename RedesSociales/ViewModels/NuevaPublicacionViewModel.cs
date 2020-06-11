using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using RedesSociales.Models;
using RedesSociales.Validations.Base;
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
        public ICommand TomarFoto { get; set; }
        public ICommand SeleccionarFoto { get; set; }
        public ICommand Enviar { get; set; }
        private ImageSource imagen { get; set; }
        private MemoryStream memoryStream { get; set; }

        #endregion Atributes
        #endregion Properties

        public NuevaPublicacionViewModel()
        {
            TomarFoto = new Command(async () => await TomarFotoCommand(), () => true);
            SeleccionarFoto = new Command(async () => await SeleccionarFotoCommand(), () => true);
            Enviar = new Command(async () => await EnviarCommand(), () => true);
            memoryStream = new MemoryStream();
        }

        public ImageSource Imagen
        {
            get { return imagen; }
            set { imagen = value; OnPropertyChanged(); }
        }

        private async Task TomarFotoCommand()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Photos",
                SaveToAlbum = false,
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Front
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
        }

        private async Task EnviarCommand()
        {
            string base64String = Convert.ToBase64String(memoryStream.ToArray());
            memoryStream = new MemoryStream();

            // Aqui falta todo lo relacionado a subir la imagen al servidor
            HttpClient client = new HttpClient();
            JObject jsonObject = new JObject();

            jsonObject.Add("img64", base64String);
            string json = JsonConvert.SerializeObject(jsonObject);
        }
    }
}
