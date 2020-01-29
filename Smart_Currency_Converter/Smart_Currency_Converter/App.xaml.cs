using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Smart_Currency_Converter
{
    public partial class App : Application
    {
        public static NavigationPage NavigationObj;

        public App()
        {
            InitializeComponent();

            NavigationObj = new NavigationPage(new MainPage());
            MainPage = NavigationObj;
            //MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
