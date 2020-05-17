using Xamarin.Forms;
using Model.Smart_Currency_Converter;

namespace ViewModel.Result
{
    public class ResultPageViewModel
    {
        public Command Convert { get; }

        public ResultPageViewModel()
        {
            Convert = new Command(TempMethod);
        }

        private async void TempMethod()
        {
            CurrencyAcronymEnum a = new CurrencyAcronymEnum();
            //string baseCurrency = "EUR";
            //string targetCurrency = "USD";

            //Converter converter = new Converter();
            //decimal r = await converter.Convert(10, baseCurrency, targetCurrency);
        }
    }
}