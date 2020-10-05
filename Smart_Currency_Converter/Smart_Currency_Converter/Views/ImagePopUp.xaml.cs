using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Views.Smart_Currency_Converter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePopUp : ContentPage
    {
        public ImagePopUp(ImageSource imageSource)
        {
            InitializeComponent();
            TakenImage.Source = imageSource;
        }
    }
}