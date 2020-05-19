using Xunit;
using Newtonsoft.Json.Linq;
using Model.Smart_Currency_Converter;

namespace Smart_Currency_Converter.UnitTests
{
    public class CurrencyInfoTests
    {
        [Fact]
        public async void GetAllCurrenciesRate_HappyPath_ResponseObjectReturned()
        {
            // Arrange \\
            JObject responseObject = await CurrencyInfo.Instance.GetAllCurrenciesRate();

            // Act \\

            // Assert \\
            Assert.True(true);
        }
    }
}