using Xamarin.Forms;
using System.ComponentModel;
using Smart_Currency_Converter;

namespace ViewModel.HomePage
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Command OpenSmartConverterPage { get; }
        public Command OpenManualConverterPage { get; }

        public HomePageViewModel()
        {
            OpenManualConverterPage = new Command(OpenManualPage);
            OpenSmartConverterPage = new Command(OpenSmartPage);
        }

        private async void OpenManualPage() =>
            await App.NavigationObj.PushAsync(new ManualConventerPage());

        private async void OpenSmartPage() =>
            await App.NavigationObj.PushAsync(new SmartConverterPage());
    }
}
