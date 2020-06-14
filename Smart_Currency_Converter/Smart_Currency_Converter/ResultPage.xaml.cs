using Xamarin.Forms;
using ViewModel.Result;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace Smart_Currency_Converter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {
        public ResultPage(List<KeyValuePair<string, string>> itemPricePairs, ImageSource imageSource)
        {
            ResultPageViewModel.ItemPricePairs = itemPricePairs;
            ResultPageViewModel.ModalNavigation = Navigation;
            ResultPageViewModel.Image = imageSource;

            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }
    }
}