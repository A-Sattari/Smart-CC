using System;
using Android.OS;
using Android.App;
using Android.Content.PM;
using Smart_Currency_Converter.Exceptions;

namespace Smart_Currency_Converter.Droid
{
    [Activity(Label = "Smart CC", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
              ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;

                base.OnCreate(savedInstanceState);

                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
                global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);

                LoadApplication(new App());

            } catch (AnalysisApiException ex)
            {

            } catch (InternetAccessException ex)
            {

            } catch (CameraAccessException ex)
            {

            } catch (Exception ex)
            {

            }
        }
    }
}