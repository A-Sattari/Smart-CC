using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace Smart_Currency_Converter_Cloud
{
    public static class AnalyzeImage
    {
        private const string subscriptionKey = "2416c55d1d9342a5997a7c751cfc8b2f";
        private const string endpoint = "https://scc-computervision.cognitiveservices.azure.com/";

        [FunctionName("AnalyzeImage")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req, ILogger log)
        {
            log.LogInformation($"{nameof(AnalyzeImage)} Function is triggered ...");
            string responseMessage = null;

            MultipartMemoryStreamProvider inputData = await req.Content.ReadAsMultipartAsync();
            Stream imageAsStream = await inputData.Contents[0].ReadAsStreamAsync();


            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            };

            ComputerVisionClient visionClient = Authenticate(endpoint, subscriptionKey);
            ImageAnalysis results = await visionClient.AnalyzeImageInStreamAsync(imageAsStream, features);

            foreach (var caption in results.Description.Captions) {
                responseMessage = $"{caption.Text} with confidence {caption.Confidence}";
            }

            return new OkObjectResult(responseMessage);
        }


        private static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
            return client;
        }
    }
}