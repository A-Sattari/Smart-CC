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
            OpenSmartConverterPage = new Command(OpenSmartPage);
        }

        private void OpenManualPage()
        {

        }

        private async void OpenSmartPage() =>
            await App.NavigationObj.PushAsync(new SmartConverterPage());
    }
}
