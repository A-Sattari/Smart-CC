using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace Smart_Currency_Converter_Cloud
{
    public static class AnalyzeImage
    {
        private const string subscriptionKey = "2416c55d1d9342a5997a7c751cfc8b2f";
        private const string endpoint = "https://scc-computervision.cognitiveservices.azure.com/";
        private const string ANALYZE_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png";

        [FunctionName("AnalyzeImage")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation($"{nameof(AnalyzeImage)} Function is triggered ...");

            string responseMessage = null;

            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            };

            ComputerVisionClient visionClient = Authenticate(endpoint, subscriptionKey);

            ImageAnalysis results = await visionClient.AnalyzeImageAsync(ANALYZE_URL_IMAGE, features);

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
