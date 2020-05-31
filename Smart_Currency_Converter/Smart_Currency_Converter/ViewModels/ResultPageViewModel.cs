using System;
using Xamarin.Forms;
using System.Reflection;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace ViewModel.Result
{
    public class ResultPageViewModel
    {
        public static List<KeyValuePair<string, decimal>> ItemPricePairs { get; set; }
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