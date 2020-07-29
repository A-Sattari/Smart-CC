using System.Linq;
using Xamarin.Forms;
using System.Windows.Input;
using ViewModel.SmartConverter;
using System.Collections.Generic;
using Model.Smart_Currency_Converter;
using Smart_Currency_Converter.Models;

namespace ViewModel.CurrencyListModal
{
    public class CurrencyListPageViewModel
    {
        private readonly CurrencySymbolMapper symbolMapper;
        public static INavigation ModalNavigation;

        public ICommand SelectedCurrencyChanged { get; }
        public static SmartConverterViewModel SmartPageVideModel { private get; set; }
        public List<Currency> CurrenciesList { get; private set; } = new List<Currency>();

        public CurrencyListPageViewModel()
        {
            SelectedCurrencyChanged = new Command<Currency>(SelectCurrency);
            symbolMapper = new CurrencySymbolMapper();

            SetUpList();
        }

        private void SetUpList()
        {
            HashSet<string> acronyms = Cache.Instance.GetAcronyms();

            foreach (string acronym in acronyms)
            {
                if (SmartPageVideModel.BaseCurrency.Acronym != acronym && SmartPageVideModel.TargetCurrency.Acronym != acronym)
                {
                    Currency currency = new Currency
                    {
                        Name = symbolMapper.GetCurrencyNameInEnglish(acronym),
                        Acronym = acronym,
                        Symbol = symbolMapper.GetCurrencySymbol(acronym),
                        Flag = symbolMapper.GetCurrencyCountryFlag(acronym)
                    };

                    CurrenciesList.Add(currency);
                }
            }
            CurrenciesList = CurrenciesList.OrderBy(c => c.Name).ToList();
        }

        private void SelectCurrency(Currency selectedItem)
        {
            if (SmartPageVideModel.isFirstCardSelected)
            {
                SmartPageVideModel.BaseCurrency = new Currency()
                {
                    Name = selectedItem.Name,
                    Acronym = selectedItem.Acronym,
                    Symbol = selectedItem.Symbol,
                    Flag = selectedItem.Flag
                };
            } else
            {
                SmartPageVideModel.TargetCurrency = new Currency()
                {
                    Name = selectedItem.Name,
                    Acronym = selectedItem.Acronym,
                    Symbol = selectedItem.Symbol,
                    Flag = selectedItem.Flag
                };
            }

            CloseCurrencyListPageAsync();
        }

        private async void CloseCurrencyListPageAsync() => await ModalNavigation.PopModalAsync();
    }
}