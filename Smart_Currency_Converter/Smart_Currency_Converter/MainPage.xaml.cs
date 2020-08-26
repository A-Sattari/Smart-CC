using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ViewModel.SmartConverter;

namespace Smart_Currency_Converter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            MainPageViewModel.ModalNavigation = Navigation;

            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }
    }
}