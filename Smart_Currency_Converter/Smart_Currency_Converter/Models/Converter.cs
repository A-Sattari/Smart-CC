// Dynamic Enum :
// https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.enumbuilder?view=netcore-3.1
// https://stackoverflow.com/questions/725043/automatically-create-an-enum-based-on-values-in-a-database-lookup-table/792332#792332
namespace Model.Smart_Currency_Converter
{
    public class Converter
    {
        private readonly CurrencyInfo currencyInfo;

        public Converter()
        {
            currencyInfo = new CurrencyInfo();
        }

        public decimal Convert(decimal amount, string baseCurrency, string targetCurrency)
        {
            baseCurrency = "EUR";
            targetCurrency = "USD";

            decimal baseRate = GetCurrencyRate(baseCurrency, targetCurrency);
            return amount * baseRate;
        }


    #region Private Methods

        private decimal GetCurrencyRate(string baseCurrency, string targetCurrency)
        {
            decimal baseRatePerCAD = currencyInfo.GetCurrencyRate(baseCurrency).Result;
            decimal targetRatePerCAD = currencyInfo.GetCurrencyRate(targetCurrency).Result;

            return baseRatePerCAD / targetRatePerCAD;  // Base Rate based on Target
        }
    
    #endregion
    }
}