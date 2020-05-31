using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Model.Smart_Currency_Converter
{
    public class Converter
    {
        public async Task<decimal> Convert(decimal amount, string baseCurrency, string targetCurrency)
        {
            decimal baseRate = await GetCurrencyRate(baseCurrency, targetCurrency);
            return Math.Round(amount * baseRate, 2);
        }

        public async Task<List<KeyValuePair<string, decimal>>> Convert(List<KeyValuePair<string, decimal>> pairs, string baseCurrency, string targetCurrency)
        {
            List<KeyValuePair<string, decimal>> convertedPairs = new List<KeyValuePair<string, decimal>>();

            decimal baseRate = await GetCurrencyRate(baseCurrency, targetCurrency);

            foreach (KeyValuePair<string, decimal> pair in pairs)
            {
                decimal convertedAmount = pair.Value * baseRate;
                convertedAmount = Math.Round(convertedAmount, decimals: 2);
                convertedPairs.Add(new KeyValuePair<string, decimal>(pair.Key, convertedAmount));
            }

            return convertedPairs;
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