using System;
using System.IO;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.ComponentModel;
using System.Threading.Tasks;
using Smart_Currency_Converter;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using Model.Smart_Currency_Converter;

namespace ViewModel.SmartConverter
{
    public class SmartConverterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static INavigation ModalNavigation;
        private readonly ImageProcessingHelper imageProcessing;
        
        public Command TakePhoto { get; }

        public SmartConverterViewModel()
        {
            imageProcessing = new ImageProcessingHelper();
            TakePhoto = new Command(CameraButtonClickedAsync);

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
            OpenLoadingPage();

            byte[] imageByteArray = ConvertImageToByte(photo);
            
            List<KeyValuePair<string, decimal>> itemPricePairs = await imageProcessing.AnalyzeTakenPhotoAsync(imageByteArray);            
            itemPricePairs = await new Converter().Convert(itemPricePairs, "CAD", "EUR");

            ImageSource image = ImageSource.FromStream(() => new MemoryStream(imageByteArray));
            OpenResultPageAsync(itemPricePairs, image);

            File.Delete(photo.Path);
        }

        private async void OpenResultPageAsync(List<KeyValuePair<string, decimal>> itemPricePairs, ImageSource image)
        {
            await App.NavigationObj.PushAsync(new ResultPage(itemPricePairs, image));
            CloseLoadingPage();
        }

        private async void OpenLoadingPage() => await ModalNavigation.PushModalAsync(new LoadingPage());

        private async void CloseLoadingPage() => await ModalNavigation.PopModalAsync();

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
    }
}