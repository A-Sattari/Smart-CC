using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Smart_Currency_Converter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManualConventerPage : ContentPage
    {
        public ManualConventerPage()
        {
            InitializeComponent();
            //TODO: When the ViewModel file of this page added, add the EnsureCacheIsUpToDate() method
        }
    }
}