using System.Globalization;

namespace Model.Smart_Currency_Converter
{
    public readonly struct CurrencySymbolMapper
    {
        public readonly string GetCurrencySymbol(string currencyAcr)
        {
            RegionInfo regionInfo = GetRegionInfo(currencyAcr);
            if (regionInfo == null)
                return string.Empty;

            return regionInfo.CurrencySymbol;
        }

        public readonly string GetCurrencyNameInEnglish(string currencyAcr)
        {
            RegionInfo regionInfo = GetRegionInfo(currencyAcr);
            if (regionInfo == null)
                return currencyAcr;

            return regionInfo.CurrencyEnglishName;
        }

        private readonly RegionInfo GetRegionInfo(string currencyAcr)
        {
            RegionInfo regionInfo = currencyAcr switch
            {
                "CAD" => new RegionInfo("en-CA"),
                "HKD" => new RegionInfo("zh-HK"),
                "ISK" => new RegionInfo("is-IS"),
                "PHP" => new RegionInfo("en-PH"),
                "DKK" => new RegionInfo("da-DK"),
                "HUF" => new RegionInfo("hu-HU"),
                "CZK" => new RegionInfo("cs-CZ"),
                "AUD" => new RegionInfo("en-AU"),
                "RON" => new RegionInfo("ro-RO"),
                "SEK" => new RegionInfo("sv-SE"),
                "IDR" => new RegionInfo("id-ID"),
                "INR" => new RegionInfo("en-IN"),
                "BRL" => new RegionInfo("pt-BR"),
                "RUB" => new RegionInfo("ru-RU"),
                "HRK" => new RegionInfo("hr-HR"),
                "JPY" => new RegionInfo("ja-JP"),
                "THB" => new RegionInfo("th-TH"),
                "CHF" => new RegionInfo("fr-CH"),
                "SGD" => new RegionInfo("zh-SG"),
                "PLN" => new RegionInfo("pl-PL"),
                "BGN" => new RegionInfo("bg-BG"),
                "TRY" => new RegionInfo("tr-TR"),
                "CNY" => new RegionInfo("zh-CN"),
                "NOK" => new RegionInfo("nb-NO"),
                "NZD" => new RegionInfo("en-NZ"),
                "ZAR" => new RegionInfo("en-ZA"),
                "MXN" => new RegionInfo("es-MX"),
                "ILS" => new RegionInfo("he-IL"),
                "GBP" => new RegionInfo("en-GB"),
                "KRW" => new RegionInfo("ko-KR"),
                "MYR" => new RegionInfo("ms-MY"),
                "EUR" => new RegionInfo("fr-FR"),
                "IRR" => new RegionInfo("fa-IR"),
                "SAR" => new RegionInfo("ar-SA"),
                "AED" => new RegionInfo("ar-AE"),
                "QAR" => new RegionInfo("ar-QA"),
                "IQD" => new RegionInfo("ar-IQ"),
                "MAD" => new RegionInfo("ar-MA"),
                "EGP" => new RegionInfo("ar-EG"),
                _ => null   // Default case
            };

            return regionInfo;
        }
    }
}