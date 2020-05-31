using Xamarin.Forms;
using ViewModel.Result;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace Smart_Currency_Converter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {
        public ResultPage(List<KeyValuePair<string, decimal>> itemPricePairs)
        {
            ResultPageViewModel.ItemPricePairs = itemPricePairs;
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }
    }
}