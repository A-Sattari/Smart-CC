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

    #region Private Methods

        private ImageAnalysisResultObject ParseImageContent(string imageContent) =>
            JsonConvert.DeserializeObject<ImageAnalysisResultObject>(imageContent);

        private List<KeyValuePair<string, decimal>> PurifyImageContent(List<Lines> itemsAndPrice)
        {
            var itemPricePair = new List<KeyValuePair<string, decimal>>();
            StringBuilder item = new StringBuilder();
            Regex containsNumberRegEx = new Regex(@"\d+");
            Match regExMatcher;

            for (int x = 0; x < itemsAndPrice.Count; x++) {

                string rawValue = itemsAndPrice[x].Text;
                regExMatcher = containsNumberRegEx.Match(rawValue);

                // The string contains a number
                if (regExMatcher.Success) {

                    var itemAndPrice = PurifyStringWithNumber(rawValue, item, regExMatcher);
                    itemPricePair.Add(itemAndPrice);
                    item.Clear(); // We assume that when a number is found, we have the entire item name

                } else {
                    item.Append(rawValue).Append(' ');
                }
            }

            return itemPricePair;
        }

        private KeyValuePair<string, decimal> PurifyStringWithNumber(string rawValue, StringBuilder item, Match regExMatcher)
        {
            decimal price;

            // The entire string is not a number
            if (regExMatcher.Index != 0) {

                int numberIndex = regExMatcher.Index;

                string text = rawValue.Substring(0, numberIndex);
                string priceString = rawValue.Substring(numberIndex);
                bool isNumber = decimal.TryParse(priceString, out price); //TODO: If can't convert -> Unknown error occurred

                item.Append(text).Append(' ');

            } else {
                bool isNumber = decimal.TryParse(rawValue, out price); //TODO: If can't convert -> Unknown error occurred
            }

            return new KeyValuePair<string, decimal>(item.ToString(), price);
        }

    #endregion
    }
}