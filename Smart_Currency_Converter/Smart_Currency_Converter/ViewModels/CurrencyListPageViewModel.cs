using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ViewModel.CurrencyListModal
{
    public class CurrencyListPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> CurrenciesName { get; set; }

        public CurrencyListPageViewModel()
        {
            CurrenciesName = new List<string>()
            {
                "Currency 1",
                "Currency 2",
                "Currency 3",
                "Currency 4",
                "Currency 5",
                "Currency 6",
                "Currency 7",
                "Currency 9",
                "Currency 10",
                "Currency 11"
            };
        }
    }
}
