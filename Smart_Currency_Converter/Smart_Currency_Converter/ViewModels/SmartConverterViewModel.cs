using System;
using System.IO;
using Plugin.Media;
using Xamarin.Forms;
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

namespace ViewModel.SmartConverter
{
    public class SmartConverterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static INavigation ModalNavigation;
        private readonly ImageProcessingHelper imageProcessing;
        public bool isFirstCardSelected = false;
        private CurrencyObject card1currency;
        private CurrencyObject card2currency;

        public CurrencyObject FirstCard
        { 
            get => card1currency;
            set 
            {
                card1currency = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(FirstCard)));
            }
        }
        
        public CurrencyObject SecondCard
        {
            get => card2currency;
            set 
            {
                card2currency = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SecondCard)));
            }
        }

        public ICommand CardClicked { get; }
        public ICommand TakePhoto { get; }

        public SmartConverterViewModel()
        {
            imageProcessing = new ImageProcessingHelper();
            CardClicked = new Command<string>(OpenCurrencyListPageAsync);
            TakePhoto = new Command(CameraButtonClickedAsync);

            card1currency = new CurrencyObject()
            {
                Name = "Canadian Dollar",
                Accronym = "CAD",
                Symbol = "https://images-na.ssl-images-amazon.com/images/I/614JLqsvMoL._AC_SX679_.jpg"
            };

            card2currency = new CurrencyObject()
            {
                Name = "Yen",
                Accronym = "JPY",
                Symbol = "https://images-na.ssl-images-amazon.com/images/I/614JLqsvMoL._AC_SX679_.jpg"
            };

            EnsureCacheIsUpToDate();
        }

        private async void CameraButtonClickedAsync()
        {
            if (!CrossMedia.Current.IsCameraAvailable)
                throw new Exception("Camera Unavailable");

            MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                Name = $"Smart-CC_{DateTime.Now:yy-MMdd-Hmms}",
                SaveToAlbum = true
            });

            OpenLoadingPageAsync();

            byte[] imageByteArray = ConvertImageToByte(photo);

            List<KeyValuePair<string, string>> convertedPairs = await PerformConversionAsync(imageByteArray);

            OpenResultPageAsync(convertedPairs, GetImageSourceObj(imageByteArray));

            File.Delete(photo.Path);
        }

        private async Task<List<KeyValuePair<string, string>>> PerformConversionAsync(byte[] imageByteArray)
        {
            List<KeyValuePair<string, decimal>> itemPricePairs = await imageProcessing.AnalyzeTakenPhotoAsync(imageByteArray);
            return (await new Converter().Convert(itemPricePairs, baseCurrency: card1currency.Accronym, targetCurrency: card2currency.Accronym));
        }

        private ImageSource GetImageSourceObj(byte[] imageArray) => ImageSource.FromStream(() => new MemoryStream(imageArray));

        private byte[] ConvertImageToByte(MediaFile photo)
        {
            byte[] imageArray = null;

            try {
                using (MemoryStream memory = new MemoryStream()) {

                    Stream stream = photo.GetStream();
                    stream.CopyTo(memory);
                    imageArray = memory.ToArray();
                }

            } catch (OutOfMemoryException ex) {
                Console.Error.WriteLine(ex);
            }

            if (imageArray == null || imageArray.Length == 0)
                throw new NullReferenceException(nameof(imageArray));

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

        private async void OpenResultPageAsync(List<KeyValuePair<string, string>> itemPricePairs, ImageSource image)
        {
            await App.NavigationObj.PushAsync(new ResultPage(itemPricePairs, image));
            CloseLoadingPageAsync();
        }

        #endregion
    }
}