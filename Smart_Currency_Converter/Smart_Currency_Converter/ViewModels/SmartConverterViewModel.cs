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
using Model.Smart_Currency_Converter;
using Smart_Currency_Converter.Models;
using ModalPages.Smart_Currency_Converter;
using Smart_Currency_Converter.Exceptions;
using Smart_Currency_Converter.InformativeViews;
using Microsoft.AppCenter.Crashes;

namespace ViewModel.SmartConverter
{
    public class SmartConverterViewModel : INotifyPropertyChanged
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

        public SmartConverterViewModel()
        {
            CurrencySymbolMapper symbolMapper = new CurrencySymbolMapper();
            imageProcessing = new ImageProcessingService();
            converter = new Converter();

            CardClicked = new Command<string>(OpenCurrencyListPageAsync);
            SwapCards = new Command(SwapCard);
            TakePhoto = new Command(TakePhotoAction);

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

            EnsureCacheIsUpToDate();
            GenerateExchangeRateMessage();
        }

        private async void TakePhotoAction()
        {
            try
            {
                await TakePhotoButtonClickedAsync();

            } catch (AnalysisApiException ex)
            {
                Crashes.TrackError(ex);
                ErrorPromptView.Display(ex.Message);

            } catch (InternetAccessException ex)
            {
                Crashes.TrackError(ex);

            } catch (CameraAccessException ex)
            {
                Crashes.TrackError(ex);
                ErrorPromptView.Display(ex.Message);

            } catch (Exception ex)
            {
                Crashes.TrackError(ex);
                ErrorPromptView.Display(ex.Message);
            }
        }

        [System.Diagnostics.Conditional("DEBUG")]
        void ExceptionThrower()
        {
            throw new AnalysisApiException();
        }

        private async Task TakePhotoButtonClickedAsync()
        {
            if (!CrossMedia.Current.IsCameraAvailable)
                throw new CameraAccessException();

            MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                Name = $"Smart-CC_{DateTime.Now:yy-MMdd-Hmms}"
            });

            OpenLoadingPageAsync();

            byte[] imageByteArray = ConvertImageToByte(photo);

            List<KeyValuePair<string, decimal>> convertedPairs = await PerformConversionAsync(imageByteArray);

            OpenResultPageAsync(convertedPairs, GetImageSourceObj(imageByteArray));
            File.Delete(photo.Path);
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

            if (string.IsNullOrEmpty(exchangeRateMessage))
                exchangeRateMessage = message;
            else
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

        private async void EnsureCacheIsUpToDate()
        {
            if (!Cache.Instance.CacheIsUpToDate && Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Action updateCacheData = Cache.Instance.UpdateCacheData;
                await Task.Run(updateCacheData);
            }
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

        private async void OpenLoadingPageAsync() => await ModalNavigation.PushModalAsync(new LoadingPage());
        
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