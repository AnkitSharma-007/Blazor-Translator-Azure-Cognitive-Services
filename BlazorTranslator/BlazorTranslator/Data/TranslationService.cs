using BlazorTranslator.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTranslator.Data
{
    public class TranslationService
    {
        static string subscriptionKey;
        static string apiEndpoint;
        static string location;

        public TranslationService()
        {
            subscriptionKey = "e380526a44b14b7b8e3d0a14455c783a";
            apiEndpoint = "https://api.cognitive.microsofttranslator.com/";
            location = "southeastasia";
        }

        public async Task<TranslationResult[]> GetTranslatation(string textToTranslate, string targetLanguage)
        {
            string route = $"translate?api-version=3.0&to={targetLanguage}";
            string requestUri = apiEndpoint + route;

            string result = await TranslateText(requestUri, textToTranslate);

            TranslationResult[] translationResult = JsonConvert.DeserializeObject<TranslationResult[]>(result);

            return translationResult;
        }

        static async Task<string> TranslateText(string requestUri, string inputText)
        {
            string result = string.Empty;
            object[] body = new object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);

            using var client = new HttpClient();
            using var request = new HttpRequestMessage();

            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(requestUri);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            request.Headers.Add("Ocp-Apim-Subscription-Region", location);

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }

            return result;
        }

        public async Task<AvailableLanguage> GetAvailableLanguages()
        {
            string endpoint = "https://api.cognitive.microsofttranslator.com/languages?api-version=3.0&scope=translation";

            var client = new HttpClient();
            using var request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri(endpoint);

            var response = await client.SendAsync(request).ConfigureAwait(false);

            string result = await response.Content.ReadAsStringAsync();
            AvailableLanguage deserializedOutput = JsonConvert.DeserializeObject<AvailableLanguage>(result);

            return deserializedOutput;
        }
    }
}