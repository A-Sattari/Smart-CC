using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Model.Smart_Currency_Converter
{
    public class ImageProcessingHelper
    {
        public byte[] PhotoInByte { get; private set; }

        public void PostImageForAnalysis(MediaFile photo)
        {
            PhotoInByte = ConvertImageToByte(photo);
        }

        //TODO: Add try & catch
        private byte[] ConvertImageToByte(MediaFile photo)
        {
            byte[] imageArray = null;

            using (MemoryStream memory = new MemoryStream()) {

                Stream stream = photo.GetStream();
                stream.CopyTo(memory);
                imageArray = memory.ToArray();
            }

            return imageArray;
        }
    }
}
