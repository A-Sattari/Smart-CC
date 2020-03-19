using Plugin.Media;
using Xamarin.Forms;
using System.ComponentModel;

namespace ViewModel.SmartConverter
{
    public class SmartConverterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Command TakePhoto { get; }
        private ImageSource image;
        private string imagePath;

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
            imagePath = photo.Path;

            ImageDisplay = ImageSource.FromStream(() => { return photo.GetStream(); });
        }
    }
}