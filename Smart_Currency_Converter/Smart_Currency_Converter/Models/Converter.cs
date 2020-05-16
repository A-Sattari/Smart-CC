using System;
using System.Threading.Tasks;
// Dynamic Enum :
// https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.enumbuilder?view=netcore-3.1
// https://stackoverflow.com/questions/725043/automatically-create-an-enum-based-on-values-in-a-database-lookup-table/792332#792332

namespace Model.Smart_Currency_Converter
{
    public class Converter
    {
        public async Task<decimal> Convert(decimal amount, string baseCurrency, string targetCurrency)
        {
            decimal baseRate = await GetCurrencyRate(baseCurrency, targetCurrency);
            return amount * baseRate;
        }


    #region Private Methods

        private async Task<decimal> GetCurrencyRate(string baseCurrency, string targetCurrency)
        {
            decimal baseRatePerCAD = await CurrencyInfo.Instance.GetCurrencyRateAsync(baseCurrency);
            decimal targetRatePerCAD = await CurrencyInfo.Instance.GetCurrencyRateAsync(targetCurrency);

            if (baseRatePerCAD.Equals(0))
                throw new DivideByZeroException(nameof(baseRatePerCAD));

            return targetRatePerCAD / baseRatePerCAD;  // Base Rate based on Target
        }
    
    #endregion
    }
}