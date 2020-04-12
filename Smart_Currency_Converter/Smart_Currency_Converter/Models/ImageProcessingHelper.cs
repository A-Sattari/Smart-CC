using System;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

            string imageContent = await response.Content.ReadAsStringAsync();

            ImageAnalysisResultObject result = ParseImageContent(imageContent);
            
            if (!result.Status.Equals("Succeeded"))
                throw new Exception("Not successful");

            List<Lines> itemsAndPrice = result.RecognitionResults[0].Lines;
            PurifyImageContent(itemsAndPrice);
        }

        public ImageAnalysisResultObject ParseImageContent(string imageContent) =>
            JsonConvert.DeserializeObject<ImageAnalysisResultObject>(imageContent);

        private void PurifyImageContent(List<Lines> itemsAndPrice)
        {
            Regex containsNumberRegEx = new Regex(@"\d+");
            Match regExMatcher;
            StringBuilder output = new StringBuilder();
            decimal price = decimal.Zero;
            bool newItemFound = false; // Once we reach a number (price), we assume that's the end of that item in the list
            var itemPricePair = new List<KeyValuePair<string, decimal>>();

            for (int x = 0; x < itemsAndPrice.Count; x++) {

                string value = itemsAndPrice[x].Text;
                regExMatcher = containsNumberRegEx.Match(value);

                // The string contains a number
                if (regExMatcher.Success) {

                    // The entire string is not a number
                    if (regExMatcher.Index != 0) {

                        int numberIndex = regExMatcher.Index;

                        string text = value.Substring(0, numberIndex);
                        string priceString = value.Substring(numberIndex);
                        bool isNumber = decimal.TryParse(priceString, out price); //TODO: If can't convert -> Unknown error occurred

                        output.Append(text + " ");
                        Console.WriteLine($"\nPrice: {price}\n\n");
                        newItemFound = true;
                    
                    } else { // We assume the entire string is a number

                        bool isNumber = decimal.TryParse(value, out price); //TODO: If can't convert -> Unknown error occurred
                        Console.WriteLine($"\nPrice: {price}\n\n");
                        newItemFound = true;
                    }

                } else {
                    output.Append(value + " ");
                    Console.WriteLine(value);
                }

                if (newItemFound) {

                    itemPricePair.Add(new KeyValuePair<string, decimal>(output.ToString(), price));
                    output.Clear();
                    newItemFound = false;
                }
            }
        }
    }
}