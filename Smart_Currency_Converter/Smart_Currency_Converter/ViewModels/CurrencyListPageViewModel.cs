using System.ComponentModel;
using System.Collections.Generic;
using Model.Smart_Currency_Converter;
using Xamarin.Forms;
using System.Windows.Input;
using ViewModel.SmartConverter;
using Smart_Currency_Converter.Models;

namespace ViewModel.CurrencyListModal
{
    public class CurrencyListPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly CurrencySymbolMapper symbolMapper;
        public static INavigation ModalNavigation;
        public ICommand SelectedCurrencyChanged { get; }

        public List<CurrencyObject> CurrenciesList { get; set; } = new List<CurrencyObject>();

        public CurrencyListPageViewModel()
        {
            SelectedCurrencyChanged = new Command<CurrencyObject>(SelectCurrency);
            symbolMapper = new CurrencySymbolMapper();

            SetUpList();
        }

        private void SelectCurrency(CurrencyObject selectedItem)
        {
            // Send the selected to Smart page [Determine which card is selected]
            SmartConverterViewModel.FirstCard.Name = selectedItem.Name;
            SmartConverterViewModel.FirstCard.Accronym = selectedItem.Accronym;
            SmartConverterViewModel.FirstCard.Symbol = selectedItem.Symbol;

            // Close the modal
            CloseCurrencyListPageAsync();
        }

        private void SetUpList()
        {
            HashSet<string> acronyms = Cache.Instance.GetAcronyms();

            if (CurrenciesList.Count != acronyms.Count)
            {
                foreach (string acronym in acronyms)
                {
                    string currencyName = symbolMapper.GetCurrencyNameInEnglish(acronym);

                    CurrencyObject currency = new CurrencyObject
                    {
                        Name = $"{currencyName}  ({acronym})",
                        Accronym = "(ACR)",
                        Symbol = "https://images-na.ssl-images-amazon.com/images/I/614JLqsvMoL._AC_SX679_.jpg"
                    };

                    CurrenciesList.Add(currency);
                }
            }
        }

        private async void CloseCurrencyListPageAsync() => await ModalNavigation.PopModalAsync();
    }
}