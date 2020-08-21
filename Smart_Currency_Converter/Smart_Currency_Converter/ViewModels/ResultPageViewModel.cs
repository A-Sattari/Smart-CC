using System;
using Xamarin.Forms;
using System.Reflection;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using Smart_Currency_Converter;
using System.Collections.Generic;

namespace ViewModel.Result
{
    public class ResultPageViewModel
    {
        public static INavigation ModalNavigation;
        public static ImageSource Image;
        public static string TargetSymbol;
        public static List<KeyValuePair<string, decimal>> ItemPricePairs { get; set; }

        public Command ShowTakenPhoto { get; }
        public Command RetakePhoto { get; }

        public ResultPageViewModel()
        {
            ShowTakenPhoto = new Command(ShowTakenPhotoAction);
            RetakePhoto = new Command(OpenSmartConverterPage);
        }

        private async void ShowTakenPhotoAction()
        {
            try
            {
                await DisplayTakenPhoto();
            } catch (Exception ex)
            {
            }
        }

        private async Task DisplayTakenPhoto() => await ModalNavigation.PushModalAsync(new ImagePopUp(Image));

        private async void OpenSmartConverterPage() => await App.NavigationObj.PopAsync();
    }


    /// <summary>
    /// This class enables the XAML components in ResultPage.xaml to use embedded images that are common for all platforms.
    /// </summary>
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
                return null;

            ImageSource imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

            return imageSource;
        }
    }
}