using System;
using System.Net.Http;

namespace Model.Smart_Currency_Converter
{
    public class ImageProcessingHelper
    {
        private readonly Uri azureFunctionUrl = new Uri("https://analyzeimage.azurewebsites.net/api/AnalyzeImage");
        //private readonly string localUrl = "http://192.168.1.9:7071/api/AnalyzeImage";

        public async void PostImageForAnalysis(byte[] imageByteArray)
        {
            HttpContent content = new ByteArrayContent(imageByteArray);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            using var multipartFormData = new MultipartFormDataContent();
            multipartFormData.Add(content);

            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(azureFunctionUrl, multipartFormData);

            string imageContext = await response.Content.ReadAsStringAsync();
        }
    }
}
