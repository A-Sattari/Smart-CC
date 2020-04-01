// https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/quickstarts/csharp-hand-text?tabs=version-2
using System;
using System.Linq;
using Azure.Identity;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Smart_Currency_Converter_Cloud
{
    public static class AnalyzeImage
    {
        private const string SubscriptionSecretName = "Computer-Vision-SubscriptionKey";
        private const string EndpointSecretName = "Computer-Vision-Endpoint";

        private static readonly string subscriptionKey = GetAzureKeyVaultValue(SubscriptionSecretName);
        private static readonly string endpoint = GetAzureKeyVaultValue(EndpointSecretName);
        private static readonly string uri = endpoint + "vision/v2.1/read/core/asyncBatchAnalyze";

        [FunctionName("AnalyzeImage")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req, ILogger log)
        {
            log.LogInformation($"{nameof(AnalyzeImage)} Function is triggered ...");            
            string responseMessage = null;

            MultipartMemoryStreamProvider inputData = await req.Content.ReadAsMultipartAsync();
            byte[] imageAsByteArray = await inputData.Contents[0].ReadAsByteArrayAsync();
            // Stream imageAsStream = await inputData.Contents[0].ReadAsStreamAsync();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            HttpResponseMessage httpResponse;
            string resultUri = null;   // Stores the URI of the second REST API method, returned by the first REST API method.

            // Adds the image array as the request body
            using (ByteArrayContent content = new ByteArrayContent(imageAsByteArray))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // First REST API method, Batch Read, starts the async process to analyze the written text in the image
                httpResponse = await client.PostAsync(uri, content);
            }

            if (httpResponse.IsSuccessStatusCode) {
            
                resultUri = httpResponse.Headers.GetValues("Operation-Location").FirstOrDefault();
            
            } else {
                string errorString = await httpResponse.Content.ReadAsStringAsync();
                // TODO: Log error message and tell the user something
            }

            int i = 0;
            // If the first REST API method completes successfully, the second REST API method retrieves the text written in the image.
            do {
                System.Threading.Thread.Sleep(1000);

                httpResponse = await client.GetAsync(resultUri);
                responseMessage = await httpResponse.Content.ReadAsStringAsync();
                
                ++i;
            
            } while (i < 10 && responseMessage.IndexOf("\"status\":\"Succeeded\"") == -1);


            return new OkObjectResult(responseMessage);
        }


        private static string GetAzureKeyVaultValue(string secretName)
        {
            string secretValue = null;

            try {
                SecretClient client = new SecretClient(vaultUri: new Uri("https://msft-ai-credentials.vault.azure.net/"), credential: new DefaultAzureCredential());

                KeyVaultSecret secretClient = client.GetSecret(secretName).Value;
                secretValue = secretClient.Value;

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return secretValue;
        }
    }
}