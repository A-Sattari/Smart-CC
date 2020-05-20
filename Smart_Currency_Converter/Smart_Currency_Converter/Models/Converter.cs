using System;
using System.Threading.Tasks;

namespace Model.Smart_Currency_Converter
{
    public class Converter
    {
        public async Task<decimal> Convert(decimal amount, string baseCurrency, string targetCurrency)
        {
            decimal baseRate = await GetCurrencyRate(baseCurrency, targetCurrency);
            return amount * baseRate;
        }

        private async Task<decimal> GetCurrencyRate(string baseCurrency, string targetCurrency)
        {
            decimal baseRatePerCAD = await CurrencyInfo.Instance.GetCurrencyRateAsync(baseCurrency);
            decimal targetRatePerCAD = await CurrencyInfo.Instance.GetCurrencyRateAsync(targetCurrency);

            if (baseRatePerCAD.Equals(0))
                throw new DivideByZeroException(nameof(baseRatePerCAD));

            return targetRatePerCAD / baseRatePerCAD;
        }
    }
}