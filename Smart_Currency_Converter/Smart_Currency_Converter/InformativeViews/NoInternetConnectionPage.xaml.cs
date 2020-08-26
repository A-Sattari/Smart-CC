using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InformativeViews.Smart_Currency_Converter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoInternetConnectionPage : ContentPage
    {
        public NoInternetConnectionPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }
    }
}