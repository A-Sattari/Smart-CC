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

        // TODO: Code below is for debugging purposes. To be REMOVED
        public Command OpenResult { get; }

        public HomePageViewModel()
        {
            OpenManualConverterPage = new Command(OpenManualPage);
            OpenSmartConverterPage = new Command(OpenSmartPage);
            // TODO: Code below is for debugging purposes. To be REMOVED
            OpenResult = new Command(OpenResultPage);
        }

        private async void OpenManualPage() =>
            await App.NavigationObj.PushAsync(new ManualConventerPage());

        private async void OpenSmartPage() =>
            await App.NavigationObj.PushAsync(new SmartConverterPage());

        // TODO: Code below is for debugging purposes. To be REMOVED
        private async void OpenResultPage() =>
            await App.NavigationObj.PushAsync(new ResultPage());
    }
}