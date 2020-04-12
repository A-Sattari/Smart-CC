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
            AppCenter.Start("android=0b00a544-a123-4594-a121-05ed4b114df2;" +
                  "uwp={Your UWP App secret here};" +
                  "ios=ios=ae6ad1f3-9d18-46e3-875e-3b0a70dd1c87;",
                  typeof(Analytics), typeof(Crashes));
        }
    }
}
