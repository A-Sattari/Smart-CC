using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Smart_Currency_Converter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SmartConverterPage : ContentPage
    {
        public SmartConverterPage()
        {
            InitializeComponent();
        }

        private async void CameraButton_Clicked(object sender, System.EventArgs e)
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
                PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
        }
    }
}