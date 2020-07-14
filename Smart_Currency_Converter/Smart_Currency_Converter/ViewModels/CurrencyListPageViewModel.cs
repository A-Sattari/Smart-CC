using Xamarin.Forms;
using System.Windows.Input;
using System.ComponentModel;
using ViewModel.SmartConverter;
using System.Collections.Generic;
using Model.Smart_Currency_Converter;
using Smart_Currency_Converter.Models;

namespace ViewModel.CurrencyListModal
{
    public class CurrencyListPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly CurrencySymbolMapper symbolMapper;
        public static INavigation ModalNavigation;

        public ICommand SelectedCurrencyChanged { get; }
        public static SmartConverterViewModel SmartPageVideModel { private get; set; }
        public List<CurrencyObject> CurrenciesList { get; set; } = new List<CurrencyObject>();

        public CurrencyListPageViewModel()
        {
            SelectedCurrencyChanged = new Command<CurrencyObject>(SelectCurrency);
            symbolMapper = new CurrencySymbolMapper();

            SetUpList();
        }

        private void SelectCurrency(CurrencyObject selectedItem)
        {
            if (SmartPageVideModel.isFirstCardSelected)
            {
                SmartPageVideModel.FirstCard = new CurrencyObject()
                {
                    Name = selectedItem.Name,
                    Accronym = selectedItem.Accronym,
                    Symbol = selectedItem.Symbol
                };
            } 
            else
            {
                SmartPageVideModel.SecondCard = new CurrencyObject()
                {
                    Name = selectedItem.Name,
                    Accronym = selectedItem.Accronym,
                    Symbol = selectedItem.Symbol
                };
            }

            CloseCurrencyListPageAsync();
        }

        private void SetUpList()
        {
            HashSet<string> acronyms = Cache.Instance.GetAcronyms();

            if (CurrenciesList.Count != acronyms.Count)
            {
                foreach (string acronym in acronyms)
                {
                    if (SmartPageVideModel.FirstCard.Accronym != acronym && SmartPageVideModel.SecondCard.Accronym != acronym)
                    {
                        string currencyName = symbolMapper.GetCurrencyNameInEnglish(acronym);

                        CurrencyObject currency = new CurrencyObject
                        {
                            Name = currencyName,
                            Accronym = acronym,
                            Symbol = "https://images-na.ssl-images-amazon.com/images/I/614JLqsvMoL._AC_SX679_.jpg"
                        };

                        CurrenciesList.Add(currency);
                    }
                }
            }
        }

        private async void CloseCurrencyListPageAsync() => await ModalNavigation.PopModalAsync();
    }
}