using Xamarin.Forms;
using System.Collections.Generic;
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
            HashSet<string> set = Cache.Instance.GetAcronyms();

            string baseCurrency = "AUD";
            string targetCurrency = "USD";

            Converter converter = new Converter();
            decimal r = await converter.Convert(25.45M, baseCurrency, targetCurrency);
        }
    }
}