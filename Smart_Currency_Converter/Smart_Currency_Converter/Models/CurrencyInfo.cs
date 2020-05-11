using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Model.Smart_Currency_Converter
{
    public class CurrencyInfo
    {
        private const string CURRENCY_RATE_URL = "https://api.exchangeratesapi.io/latest";

        public async void GetCurrenciesRateAsync()
        {
            JObject currencies = await GetAllCurrenciesRate();
            string latestDate = currencies.GetValue("date").ToString();
            JToken currencyAcronym = currencies.GetValue("rates");
            Dictionary<string, decimal> currencyRatePair = currencyAcronym.ToObject<Dictionary<string, decimal>>();
        }


    #region Private Methods

        private Task<JObject> GetAllCurrenciesRate(string baseCurrency = "CAD")
        {
            string parameter = $"base={baseCurrency}";
            return GetCurrencyRateAsync(parameter);
        }

        private Task<JObject> GetSpecificCurrencyRate(string baseCurrency, string toCurrency)
        {
            if (string.IsNullOrEmpty(baseCurrency) || string.IsNullOrEmpty(toCurrency))
                throw new System.ArgumentNullException();

            string parameters = $"base={baseCurrency}&symbols={toCurrency}";
            return GetCurrencyRateAsync(parameters);
        }

        private async Task<JObject> GetCurrencyRateAsync(string parameters)
        {
            string result = null;
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{CURRENCY_RATE_URL}?{parameters}");

            // TODO: Handle of response is not successful
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }

            return JObject.Parse(result);
        }

    #endregion
    }
}