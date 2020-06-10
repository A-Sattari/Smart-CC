﻿using System;
using System.IO;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.ComponentModel;
using System.Threading.Tasks;
using Smart_Currency_Converter;
using Plugin.Media.Abstractions;
using Model.Smart_Currency_Converter;
using System.Collections.Generic;

namespace ViewModel.SmartConverter
{
    public class SmartConverterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
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
            MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                Name = $"Smart-CC_{DateTime.Now:yy-MMdd-Hmms}",
                SaveToAlbum = true
            });
            OpenLoadingPage();

            byte[] imageByteArray = ConvertImageToByte(photo);
            ImageSource image = ImageSource.FromStream(() => new MemoryStream(imageByteArray));

            List<KeyValuePair<string, decimal>> itemPricePairs = await imageProcessing.AnalyzeTakenPhotoAsync(imageByteArray);
            
            Converter converter = new Converter();
            itemPricePairs = await converter.Convert(itemPricePairs, "CAD", "EUR");

            OpenResultPage(itemPricePairs, image);
            File.Delete(photo.Path);
        }

        private void OpenResultPage(List<KeyValuePair<string, decimal>> itemPricePairs, ImageSource image) =>
                App.NavigationObj.PushAsync(new ResultPage(itemPricePairs, image));

        private async void OpenLoadingPage() => await App.NavigationObj.PushAsync(new LoadingPage());

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