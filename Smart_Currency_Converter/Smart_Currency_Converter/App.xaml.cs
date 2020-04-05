using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;

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
        }

        protected override void OnStart()
        {
            AppCenterConfiguration();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void AppCenterConfiguration()
        {
            AppCenter.Start("android=e67d81bb-1036-4e3c-a303-681b6d5a0e44;" +
                  "uwp={Your UWP App secret here};" +
                  "ios={Your iOS App secret here}",
                  typeof(Analytics), typeof(Crashes));
        }
    }
}
