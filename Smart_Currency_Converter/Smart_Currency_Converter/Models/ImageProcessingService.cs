using System;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Smart_Currency_Converter.Exceptions;

namespace Model.Smart_Currency_Converter
{
    public class ImageProcessingService
    {
        public async Task<List<KeyValuePair<string, decimal>>> AnalyzeTakenPhotoAsync(byte[] imageByteArray)
        {
            string imageContent = await PostImageContentAsync(imageByteArray);

            ImageAnalysisResultObject result = ParseImageContent(imageContent);
            
            if (!result.Status.Equals("Succeeded"))
                throw new JsonException("De-serialization Failed");

            List<Lines> itemsAndPrices = result.RecognitionResults[0].Lines;
            return PurifyImageContent(itemsAndPrices);
        }


    #region Private Methods

        private ImageAnalysisResultObject ParseImageContent(string imageContent) =>
            JsonConvert.DeserializeObject<ImageAnalysisResultObject>(imageContent);

        private async Task<string> PostImageContentAsync(byte[] imageByteArray)
        {
            Uri azureFunctionUrl = new Uri("https://analyzeimage.azurewebsites.net/api/AnalyzeImage");  // 192.168.1.9:7071

            HttpContent content = new ByteArrayContent(imageByteArray);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            using var multipartFormData = new MultipartFormDataContent();
            multipartFormData.Add(content);

            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(azureFunctionUrl, multipartFormData);

            if (!response.IsSuccessStatusCode)
                throw new AnalysisApiException();

            return await response.Content.ReadAsStringAsync();
        }

        private List<KeyValuePair<string, decimal>> PurifyImageContent(List<Lines> itemsAndPrices)
        {
            var itemPricePairs = new List<KeyValuePair<string, decimal>>();
            var item = new StringBuilder();
            Regex decimalNumberRegEx = new Regex(@"(\d+\.\d+)|(\d+\,\d+)|\d+");
            Match matchedNumber;

            for (int x = 0; x < itemsAndPrices.Count; x++) {

                string rawValue = itemsAndPrices[x].Text;
                matchedNumber = decimalNumberRegEx.Match(rawValue);
           
                // The string DOES NOT contain a number
                if (!matchedNumber.Success) {
                    item.Append(rawValue).Append(' ');

                } else {
                    PurifyStringWithNumber(rawValue, itemPricePairs, item, matchedNumber);
                }
            }

            return itemPricePairs;
        }

        private void PurifyStringWithNumber(string rawValue, List<KeyValuePair<string, decimal>> itemPricePair, StringBuilder text, Match matchedNumber)
        {
            int headIndex = 0;
            int subStringLength;

            while (matchedNumber.Success)
            {
                subStringLength = matchedNumber.Index - headIndex;
                string priceString = (matchedNumber.Value).Replace(",", ".");

                decimal price = Convert.ToDecimal(priceString);
                string item = rawValue.Substring(headIndex, subStringLength);
                
                if (text.ToString() != null) {
                    item = text.ToString() + rawValue.Substring(headIndex, subStringLength);
                    text.Clear();
                }

                itemPricePair.Add(new KeyValuePair<string, decimal>(item, price));

                headIndex += subStringLength + priceString.Length;
                matchedNumber = matchedNumber.NextMatch();
            }
        }

    #endregion
    }
}