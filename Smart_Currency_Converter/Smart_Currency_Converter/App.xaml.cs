using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Microsoft.AppCenter;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using Model.Smart_Currency_Converter;
using Views.Smart_Currency_Converter;
using Smart_Currency_Converter.InformativeViews;

namespace Smart_Currency_Converter
{
    public partial class App : Application
    {
        public static NavigationPage NavigationObj;

        public App()
        {
            InitializeComponent();

            MonkeyCache.SQLite.Barrel.ApplicationId = "SCC_Cache";

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                NavigationObj = new NavigationPage(new MainPage());
                MainPage = NavigationObj;
            } else
            {
                NavigationObj = new NavigationPage(new NoInternetConnectionPage());
                MainPage = NavigationObj;
            }
        }

        protected async override void OnStart()
        {
            AppCenterConfiguration();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Action updateCacheData = Cache.Instance.UpdateCacheData;
                await Task.Run(updateCacheData);
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                NavigationObj = new NavigationPage(new NoInternetConnectionPage());
                MainPage = NavigationObj;
            }
        }

        private void AppCenterConfiguration()
        {
            AppCenter.Start("android=0b00a544-a123-4594-a121-05ed4b114df2;" +
                    "ios=ae6ad1f3-9d18-46e3-875e-3b0a70dd1c87;",
                    typeof(Analytics), typeof(Crashes));
        }
    }
}