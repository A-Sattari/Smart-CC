namespace Model.Smart_Currency_Converter
{
    public class Converter
    {
        public Converter()
        {
            CurrencyInfo currencyInfo = new CurrencyInfo();
            //currencyInfo.GetCurrenciesRateAsync();
        }

        public decimal Convert(decimal amount, decimal conversionRate)
        {
            return amount * conversionRate;
        }
    }
}