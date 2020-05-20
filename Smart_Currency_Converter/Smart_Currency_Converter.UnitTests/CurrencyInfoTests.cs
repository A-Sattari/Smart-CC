using System;
using Xunit;
using System.Linq;
using Newtonsoft.Json.Linq;
using Model.Smart_Currency_Converter;
using System.Net.Http;

namespace Smart_Currency_Converter.UnitTests
{
    public class CurrencyInfoTests
    {
        [Fact]
        public async void GetAllCurrenciesRate_HappyPath_ResponseObjectReturned()
        {
            // Act \\
            JObject responseObject = await CurrencyInfo.Instance.GetAllCurrenciesRate();

            // Assert \\
            Assert.True(responseObject != null);
            Assert.True(responseObject.Count > 0);
        }

        [Fact]
        public async void GetAllCurrenciesRate_HappyPath_ResponseObjectContainsRates()
        {
            // Arrange \\
            const string RATES_KEY = "rates";

            // Act \\
            JObject responseObject = await CurrencyInfo.Instance.GetAllCurrenciesRate();
            JToken currencies = responseObject.GetValue(RATES_KEY);

            // Assert \\
            Assert.True(responseObject.ContainsKey(RATES_KEY));
            Assert.True(currencies.Count() > 0);
        }

        [Fact]
        public async void GetAllCurrenciesRate_HappyPath_ResponseObjectContainsDate()
        {
            // Arrange \\
            const string DATE_KEY = "date";

            // Act \\
            JObject responseObject = await CurrencyInfo.Instance.GetAllCurrenciesRate();
            string date = responseObject.GetValue(DATE_KEY).ToString();

            // Assert \\
            Assert.False(string.IsNullOrEmpty(date));

            DateTime dateObj = DateTime.Parse(date);
            Assert.True(DateTime.Compare(dateObj, DateTime.Today) == 0);
        }

        [Fact]
        public async void GetAllCurrenciesRate_NullBaseProvided_EuroUsedAsBase()
        {
            // Arrange \\
            const string BASE_KEY = "base";

            // Act \\
            JObject responseObject = await CurrencyInfo.Instance.GetAllCurrenciesRate(null);
            string baseCurrency = responseObject.GetValue(BASE_KEY).ToString();

            // Assert \\
            Assert.NotEqual("CAD", baseCurrency);
        }

        [Fact]
        public async void GetAllCurrenciesRate_InvalidBaseProvided_HttpRequestExceptionThrown()
        {
            // Arrange \\
            const string BASE_KEY = "random";

            // Act \\
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => CurrencyInfo.Instance.GetAllCurrenciesRate(BASE_KEY));

            // Assert \\
            Assert.Contains("BadRequest", exception.Message);
        }
    }
}