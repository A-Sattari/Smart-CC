using System;
using Android.OS;
using Android.App;
using Android.Content.PM;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;

namespace Smart_Currency_Converter.Droid
{
    [Activity(Label = "Smart CC", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
              ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionListener;
            TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionListener;

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);

            Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#c6aed4"));

            LoadApplication(new App());
        }

        private static void UnhandledExceptionListener(object sender, UnhandledExceptionEventArgs exceptionEven)
        {
            Crashes.TrackError(exceptionEven.ExceptionObject as Exception,
                              new Dictionary<string, string> {
                                   { "Unhandled Exception Type", nameof(UnhandledExceptionListener) }
                              });
        }

        private static void UnobservedTaskExceptionListener(object sender, UnobservedTaskExceptionEventArgs taskExceptionEvent)
        {
            Crashes.TrackError(taskExceptionEvent.Exception,
                               new Dictionary<string, string> {
                                   { "Unhandled Exception Type", nameof(UnobservedTaskExceptionListener) }
                               });
        }
    }
}