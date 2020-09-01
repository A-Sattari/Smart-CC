using System;
using System.IO;
using Plugin.Media;
using Xamarin.Forms;
using System.Reflection;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;
using Smart_Currency_Converter;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using Model.Smart_Currency_Converter;
using Views.Smart_Currency_Converter;
using Smart_Currency_Converter.Models;
using Smart_Currency_Converter.Exceptions;
using InformativeViews.Smart_Currency_Converter;

namespace ViewModel.SmartConverter
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static INavigation ModalNavigation;
        private readonly ImageProcessingService imageProcessing;
        private readonly Converter converter;
        public bool isFirstCardSelected = false;
        private Currency baseCurrency;
        private Currency targetCurrency;
        private string exchangeRateMessage;

        public Currency BaseCurrency
        { 
            get => baseCurrency;
            set 
            {
                baseCurrency = value;
                GenerateExchangeRateMessage();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(BaseCurrency)));
            }
        }
        
        public Currency TargetCurrency
        {
            get => targetCurrency;
            set 
            {
                targetCurrency = value;
                GenerateExchangeRateMessage();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(TargetCurrency)));
            }
        }

        public string ExchangeRate
        { 
            get => exchangeRateMessage;
            private set 
            { 
                exchangeRateMessage = value; 
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ExchangeRate)));
            }
        }

        public ICommand CardClicked { get; }
        public ICommand SwapCards { get; }
        public ICommand TakePhoto { get; }

        public MainPageViewModel()
        {
            CurrencySymbolMapper symbolMapper = new CurrencySymbolMapper();
            imageProcessing = new ImageProcessingService();
            converter = new Converter();

            baseCurrency = new Currency()
            {
                Name = symbolMapper.GetCurrencyNameInEnglish("CAD"),
                Acronym = "CAD",
                Symbol = symbolMapper.GetCurrencySymbol("CAD"),
                Flag = symbolMapper.GetCurrencyCountryFlag("CAD")
            };

            targetCurrency = new Currency()
            {
                Name = symbolMapper.GetCurrencyNameInEnglish("MXN"),
                Acronym = "MXN",
                Symbol = symbolMapper.GetCurrencySymbol("MXN"),
                Flag = symbolMapper.GetCurrencyCountryFlag("MXN")
            };

            GenerateExchangeRateMessage();

            CardClicked = new Command<string>(OpenCurrencyListPageAsync);
            SwapCards = new Command(SwapCard);
            TakePhoto = new Command(TakePhotoAction);
        }

        private async void TakePhotoAction()
        {
            try
            {
                await TakePhotoButtonClickedAsync();

            } catch (AnalysisApiException ex)
            {
                await ModalNavigation.PopModalAsync();
                await App.NavigationObj.PopToRootAsync();

                ErrorPromptView.Display(ex.Message);
                Crashes.TrackError(ex);

            } catch (InternetAccessException ex)
            {
                await ModalNavigation.PopModalAsync();
                await App.NavigationObj.PopToRootAsync();

                ErrorPromptView.Display(ex.Message);
                Crashes.TrackError(ex);

            } catch (CameraAccessException ex)
            {
                ErrorPromptView.Display(ex.Message);
                Crashes.TrackError(ex);

            } catch (Exception ex)
            {
                await ModalNavigation.PopModalAsync();
                await App.NavigationObj.PopToRootAsync();

                const string ErrorMessage = "Something Went Wrong!\nPlease Restart the App";
                ErrorPromptView.Display(ErrorMessage);
                Crashes.TrackError(ex);
            }
        }

        private async Task TakePhotoButtonClickedAsync()
        {
            if (!CrossMedia.Current.IsCameraAvailable)
                throw new CameraAccessException();

            if (!CheckFullInternetConnectivity())
                throw new InternetAccessException();

            MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                Name = $"Smart-CC_{DateTime.Now:yy-MMdd-Hmms}",
                PhotoSize = PhotoSize.Medium
            });

            if (photo != null)
            {
                await OpenLoadingPageAsync();

                byte[] imageByteArray = ConvertImageToByte(photo);

                List<KeyValuePair<string, decimal>> convertedPairs = await PerformConversionAsync(imageByteArray);

                OpenResultPageAsync(convertedPairs, GetImageSourceObj(imageByteArray));
                File.Delete(photo.Path);
            }
        }

        private void SwapCard()
        {
            Currency tmpCard = TargetCurrency;
            
            TargetCurrency = BaseCurrency;
            BaseCurrency = tmpCard;
        }

        private async void GenerateExchangeRateMessage()
        {
            decimal rate = await converter.Convert(decimal.One, baseCurrency, targetCurrency);
            string message = $"1 {baseCurrency.Acronym} ≈ {rate} {targetCurrency.Acronym}";

            ExchangeRate = message;
        }

        private async Task<List<KeyValuePair<string, decimal>>> PerformConversionAsync(byte[] imageByteArray)
        {
            List<KeyValuePair<string, decimal>> itemPricePairs = await imageProcessing.AnalyzeTakenPhotoAsync(imageByteArray);
            return await converter.Convert(itemPricePairs, baseCurrency, targetCurrency);
        }

        private ImageSource GetImageSourceObj(byte[] imageArray) => ImageSource.FromStream(() => new MemoryStream(imageArray));

        private byte[] ConvertImageToByte(MediaFile photo)
        {
            byte[] imageArray = null;

            using (MemoryStream memory = new MemoryStream()) {

                Stream stream = photo.GetStream();
                stream.CopyTo(memory);
                imageArray = memory.ToArray();
            }

            if (imageArray == null || imageArray.Length == 0)
                throw new NullReferenceException("Unable to convert the image to byte array");

            return imageArray;
        }

        private bool CheckFullInternetConnectivity()
        {
            Uri website = new Uri("http://google.com/generate_204");
            
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    using (var client = new System.Net.WebClient())
                    using (client.OpenRead(website))
                        return true;
                } catch
                {
                    return false;
                }
            }

            return false;
        }

    #region Interacting With Pages

        private async void OpenCurrencyListPageAsync(string selectedCard)
        {
            // This should match the Frame name in SmartConverterPage.xaml
            if (selectedCard == "FrameOne")
            {
                isFirstCardSelected = true;
            } else 
            {
                isFirstCardSelected = false;
            }

            await ModalNavigation.PushModalAsync(new CurrencyListModalPage(this));
        }

        private async Task OpenLoadingPageAsync()
        {
            if (!CheckFullInternetConnectivity())
                throw new InternetAccessException();

            await ModalNavigation.PushModalAsync(new LoadingPage());
        }
        
        private async void CloseLoadingPageAsync() => await ModalNavigation.PopModalAsync();

        private async void OpenResultPageAsync(List<KeyValuePair<string, decimal>> itemPricePairs, ImageSource image)
        {
            await App.NavigationObj.PushAsync(new ResultPage(itemPricePairs, targetCurrency.Symbol, image));
            CloseLoadingPageAsync();
        }

    #endregion
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