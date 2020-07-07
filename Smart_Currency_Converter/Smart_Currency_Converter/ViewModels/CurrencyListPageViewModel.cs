using System.ComponentModel;
using System.Collections.Generic;
using Model.Smart_Currency_Converter;

namespace ViewModel.CurrencyListModal
{
    public class CurrencyListPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly CurrencySymbolMapper symbolMapper;

        public Dictionary<string, string> AcronymFlagPair { get; private set; } = new Dictionary<string, string>();

        public CurrencyListPageViewModel()
        {
            symbolMapper = new CurrencySymbolMapper();

            SetUpList();
        }

        private void SetUpList()
        {
            HashSet<string> acronyms = Cache.Instance.GetAcronyms();

            if (AcronymFlagPair.Count != acronyms.Count)
            {
                foreach (string acronym in acronyms)
                {
                    string currencyName = symbolMapper.GetCurrencyNameInEnglish(acronym);
                    AcronymFlagPair.Add($"{currencyName}  ({acronym})", "https://images-na.ssl-images-amazon.com/images/I/614JLqsvMoL._AC_SX679_.jpg");
                }
            }
        }
    }
}