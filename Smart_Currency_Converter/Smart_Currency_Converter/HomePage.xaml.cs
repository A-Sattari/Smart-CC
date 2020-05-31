using Xamarin.Forms;
using System.ComponentModel;

namespace Smart_Currency_Converter
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }
    }
}