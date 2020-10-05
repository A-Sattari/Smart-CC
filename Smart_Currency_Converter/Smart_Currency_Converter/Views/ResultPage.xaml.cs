using Xamarin.Forms;
using ViewModel.Result;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace Views.Smart_Currency_Converter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {
        public ResultPage(List<KeyValuePair<string, decimal>> itemPricePairs, string targetCurrencySymbol, ImageSource imageSource)
        {
            ResultPageViewModel.ItemPricePairs = itemPricePairs;
            ResultPageViewModel.TargetSymbol = targetCurrencySymbol;
            ResultPageViewModel.Image = imageSource;
            ResultPageViewModel.ModalNavigation = Navigation;

            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }
    }
}