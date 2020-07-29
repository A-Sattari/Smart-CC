using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ViewModel.SmartConverter;
using ViewModel.CurrencyListModal;

namespace ModalPages.Smart_Currency_Converter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrencyListModalPage : ContentPage
    {
        public CurrencyListModalPage(SmartConverterViewModel smPageViewModel)
        {
            CurrencyListPageViewModel.ModalNavigation = Navigation;
            CurrencyListPageViewModel.SmartPageVideModel = smPageViewModel;
            InitializeComponent();
        }
    }
}