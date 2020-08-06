using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Smart_Currency_Converter.InformativeViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisconnectedInternetView : StackLayout
    {
        public DisconnectedInternetView()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }
    }
}