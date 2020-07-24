using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Xamarin.Forms;

namespace Model.Smart_Currency_Converter
{
    public readonly struct CurrencySymbolMapper
    {
        public readonly string GetCurrencySymbol(string currencyAcr)
        {
            RegionInfo regionInfo = GetRegionInfo(currencyAcr);
            if (regionInfo == null)
                return string.Empty;
            System.Console.WriteLine("\n ----> " + regionInfo.CurrencyEnglishName + ": " + regionInfo.CurrencySymbol);

            return regionInfo.CurrencySymbol;
        }

        public readonly string GetCurrencyNameInEnglish(string currencyAcr)
        {
            RegionInfo regionInfo = GetRegionInfo(currencyAcr);
            if (regionInfo == null)
                return currencyAcr;

            return regionInfo.CurrencyEnglishName;
        }

        public readonly ImageSource GetCurrencyCountryFlag(string currencyAcr)
        {
            string directory = $"{Assembly.GetCallingAssembly().GetName().Name}.Resources.Images.CountryFlags";

            string image = currencyAcr switch
            {
                "USD" => "usa-flag.png",
                "CAD" => "canada-flag.png",
                "HKD" => "hong_kong-flag.png",
                "ISK" => "iceland-flag.png",
                "PHP" => "philippines-flag.png",
                "DKK" => "denmark-flag.png",
                "HUF" => "hungary-flag.png",
                "CZK" => "czech-flag.png",
                "AUD" => "australia-flag.png",
                "RON" => "romania-flag.png",
                "SEK" => "sweden-flag.png",
                "IDR" => "indonesia-flag.png",
                "INR" => "india-flag.png",
                "BRL" => "brazil-flag.png",
                "RUB" => "russia-flag.png",
                "HRK" => "croatia-flag.png",
                "JPY" => "japan-flag.png",
                "THB" => "thailand-flag.png",
                "CHF" => "switzerland-flag.png",
                "SGD" => "singapore-flag.png",
                "PLN" => "poland-flag.png",
                "BGN" => "bulgaria-flag.png",
                "TRY" => "turkey-flag.png",
                "CNY" => "china-flag.png",
                "NOK" => "norway-flag.png",
                "NZD" => "new_zealand-flag.png",
                "ZAR" => "south_africa-flag.png",
                "MXN" => "mexico-flag.png",
                "ILS" => "israel-flag.png",
                "GBP" => "uk-flag.png",
                "KRW" => "south_korea-flag.png",
                "MYR" => "malaysia-flag.png",
                "EUR" => "euro-flag.png",
                "IRR" => "iran-flag.png",
                "SAR" => "saudi-flag.png",
                "AED" => "emirates-flag.png",
                "QAR" => "qatar-flag.png",
                "IQD" => "iraq-flag.png",
                "MAD" => "morocco-flag.png",
                "EGP" => "egypt-flag.png",
                _ => "unknown.png"   // Default case
            };
            
            return ImageSource.FromResource($"{directory}.{image}");
        }

        private readonly RegionInfo GetRegionInfo(string currencyAcr)
        {
            RegionInfo regionInfo = currencyAcr switch
            {
                "USD" => new RegionInfo("US"),
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