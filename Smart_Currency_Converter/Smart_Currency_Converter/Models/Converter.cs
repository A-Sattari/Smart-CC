using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Model.Smart_Currency_Converter
{
    public class Converter
    {
        public async Task<List<KeyValuePair<string, string>>> Convert(List<KeyValuePair<string, decimal>> pairs, string baseCurrency, string targetCurrency)
        {
            CurrencySymbolMapper symbolMapper = new CurrencySymbolMapper();
            List<KeyValuePair<string, string>> convertedPairs = new List<KeyValuePair<string, string>>();

            decimal baseRate = await GetCurrencyRate(baseCurrency, targetCurrency);

            foreach (KeyValuePair<string, decimal> pair in pairs)
            {
                decimal convertedAmount = Math.Round((pair.Value * baseRate), decimals: 2);
                string currencySymbol = symbolMapper.GetCurrencySymbol(targetCurrency);
                string currencyWithSymbol = $"{currencySymbol} {convertedAmount}";

                convertedPairs.Add(new KeyValuePair<string, string>(pair.Key, currencyWithSymbol));
            }

            return convertedPairs;
        }

        public async Task<decimal> Convert(decimal amount, string baseCurrency, string targetCurrency)
        {
            decimal baseRate = await GetCurrencyRate(baseCurrency, targetCurrency);
            return Math.Round(amount * baseRate, 2);
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