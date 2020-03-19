using System;
using Xamarin.Forms;
using System.ComponentModel;

namespace Smart_Currency_Converter
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SmartConverterPage());
        }
    }
}
