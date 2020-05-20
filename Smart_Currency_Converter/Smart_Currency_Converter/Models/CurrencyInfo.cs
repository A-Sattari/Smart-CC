using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Model.Smart_Currency_Converter
{
    public sealed class CurrencyInfo
    {
        private static readonly CurrencyInfo instance = new CurrencyInfo();
        private const string CURRENCY_RATE_URL = "https://api.exchangeratesapi.io/latest";
        
        public static CurrencyInfo Instance { get => instance; }

        private CurrencyInfo() { }

        public async Task<decimal> GetCurrencyRateAsync(string currencyAcronym)
        {
            if (string.IsNullOrEmpty(currencyAcronym))
                throw new ArgumentNullException(nameof(currencyAcronym));

            decimal rate;

            if (Cache.Instance.CacheIsUpToDate)
            {
                Dictionary<string, decimal> currenciesDic = Cache.Instance.GetData();
                
                if (!currenciesDic.TryGetValue(currencyAcronym, out rate))
                    throw new KeyNotFoundException(nameof(currencyAcronym));
            
            } else
            {
                JObject responseObject = await GetSpecificCurrencyRate(currencyAcronym);
                JToken currencies = responseObject.GetValue("rates");
                rate = currencies.Value<decimal>(currencyAcronym);

                Action updateCacheData = Cache.Instance.UpdateCacheData;
                await Task.Run(updateCacheData);
            }

            return rate;
        }

        public async Task<JObject> GetAllCurrenciesRateAsync(string baseCurrency = "CAD")
        {
            string parameter = $"base={baseCurrency}";
            return await GetRatesAsync(parameter);
        }


    #region Private Methods

        private async Task<JObject> GetSpecificCurrencyRate(string currencyAcronym)
        {
            string parameters = $"base=CAD&symbols={currencyAcronym}";
            return await GetRatesAsync(parameters);
        }

        private async Task<JObject> GetRatesAsync(string parameters)
        {
            string result;
            short retryCounter = 0;
            using HttpClient client = new HttpClient();
            HttpResponseMessage response;

            do
            {
                response = await client.GetAsync($"{CURRENCY_RATE_URL}?{parameters}");
                retryCounter++;
                System.Threading.Thread.Sleep(400);

            } while (!response.IsSuccessStatusCode && retryCounter != 10);


            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString());

            result = await response.Content.ReadAsStringAsync();

            return JObject.Parse(result);
        }

    #endregion
    }
}