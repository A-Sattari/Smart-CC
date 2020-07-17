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
        public List<CurrencyObject> CurrenciesList { get; private set; } = new List<CurrencyObject>();

        public CurrencyListPageViewModel()
        {
            SelectedCurrencyChanged = new Command<CurrencyObject>(SelectCurrency);
            symbolMapper = new CurrencySymbolMapper();

            SetUpList();
        }

        private void SetUpList()
        {
            HashSet<string> acronyms = Cache.Instance.GetAcronyms();

            foreach (string acronym in acronyms)
            {
                if (SmartPageVideModel.FirstCard.Acronym != acronym && SmartPageVideModel.SecondCard.Acronym != acronym)
                {
                    CurrencyObject currency = new CurrencyObject
                    {
                        Name = symbolMapper.GetCurrencyNameInEnglish(acronym),
                        Acronym = acronym,
                        Symbol = symbolMapper.GetCurrencyCountryFlag(acronym)
                    };

                    CurrenciesList.Add(currency);
                }
            }
            CurrenciesList = CurrenciesList.OrderBy(c => c.Name).ToList();
        }

        private void SelectCurrency(CurrencyObject selectedItem)
        {
            if (SmartPageVideModel.isFirstCardSelected)
            {
                SmartPageVideModel.FirstCard = new CurrencyObject()
                {
                    Name = selectedItem.Name,
                    Acronym = selectedItem.Acronym,
                    Symbol = selectedItem.Symbol
                };
            } else
            {
                SmartPageVideModel.SecondCard = new CurrencyObject()
                {
                    Name = selectedItem.Name,
                    Acronym = selectedItem.Acronym,
                    Symbol = selectedItem.Symbol
                };
            }

            CloseCurrencyListPageAsync();
        }

        private async void CloseCurrencyListPageAsync() => await ModalNavigation.PopModalAsync();
    }
}