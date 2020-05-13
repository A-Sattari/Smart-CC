using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Model.Smart_Currency_Converter
{
    public class CurrencyInfo
    {
        private const string CURRENCY_RATE_URL = "https://api.exchangeratesapi.io/latest";
        private bool cacheIsUpToDate = false;

        public CurrencyInfo()
        {
            if (Cache.Instance.IsEmpty())
                CacheDataAsync();

            DateTime cacheDate = DateTime.Parse(Cache.Instance.GetDataVersion());

            // If Cache date is older than today
            if (DateTime.Compare(cacheDate, DateTime.Today) < 0)
                CacheDataAsync();
        }

        public async Task<decimal> GetCurrencyRate(string currencyAcronym)
        {
            if (string.IsNullOrEmpty(currencyAcronym))
                throw new ArgumentNullException();

            decimal rate = decimal.Zero;

            if (cacheIsUpToDate)
            {
                Dictionary<string, decimal> currenciesDic = Cache.Instance.GetData();
                
                if (!currenciesDic.TryGetValue(currencyAcronym, out rate))
                    throw new ArgumentNullException(nameof(currencyAcronym));
            
            } else {
                CacheDataAsync();
                JObject responseObject = await GetSpecificCurrencyRate(currencyAcronym);
                JToken currencies = responseObject.GetValue("rates");
                //rate = currencies.Value<decimal>(currencyAcronym);
            }

            return rate;
        }


    #region Private Methods

        private Task<JObject> GetAllCurrenciesRate(string baseCurrency = "CAD")
        {
            string parameter = $"base={baseCurrency}";
            return GetCurrencyRateAsync(parameter);
        }

        private Task<JObject> GetSpecificCurrencyRate(string currencyAcronym)
        {
            string parameters = $"base=CAD&symbols={currencyAcronym}";
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

        private async void CacheDataAsync()
        {
            JObject responseObject = await GetAllCurrenciesRate();

            JToken currencies = responseObject.GetValue("rates");
            Dictionary<string, decimal> currencyRatePair = currencies.ToObject<Dictionary<string, decimal>>();
            string date = responseObject.GetValue("date").ToString();

            Cache.Instance.InsertData(currencyRatePair, date);
            cacheIsUpToDate = true;
        }

    #endregion
    }
}