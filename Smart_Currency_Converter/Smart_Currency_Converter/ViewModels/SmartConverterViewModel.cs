using System.IO;
using Plugin.Media;
using Xamarin.Forms;
using System.ComponentModel;
using Model.Smart_Currency_Converter;

namespace ViewModel.SmartConverter
{
    public class SmartConverterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Command TakePhoto { get; }
        private ImageSource image;

        public SmartConverterViewModel()
        {
            TakePhoto = new Command(CameraButtonClickedAsync);
        }

        public ImageSource ImageDisplay
        {
            get => image;
            set
            {
                image = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImageDisplay)));
            }
        }

        private async void CameraButtonClickedAsync()
        {
            var photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            ImageProcessingHelper imageProcessingHelper = new ImageProcessingHelper();
            imageProcessingHelper.PostImageForAnalysis(photo);

            byte[] imageByteArray = imageProcessingHelper.PhotoInByte;

            //TODO: Setup proper exception
            if (imageByteArray == null || imageByteArray.Length == 0)
                throw new System.Exception();

            ImageDisplay = ImageSource.FromStream(() => new MemoryStream(imageByteArray));
        }
    }
}