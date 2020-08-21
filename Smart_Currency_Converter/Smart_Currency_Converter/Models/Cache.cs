using System;
using MonkeyCache.SQLite;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Model.Smart_Currency_Converter
{
    public sealed class Cache
    {
        private const string DIC_KEY = "CurrenciesRateDicKey";
        private const string VERSION_KEY = "RatesDateKey";
        private const string UPDATE_KEY = "DataUpToDateKey";
        private const string Acronyms_Key = "CurrenciesAcronymKey";
        private const short TWENTY_DAYS = 20;

        public static Cache Instance { get; } = new Cache();

        public bool CacheIsUpToDate
        {
            get => Barrel.Current.Get<bool>(UPDATE_KEY);
        }

        private Cache()
        {
            Barrel.Current.EmptyExpired();

            if (!IsEmpty())
            {
                DateTime cacheDate = DateTime.Parse(GetContentDate());

                // If Cache date is older than today
                if (DateTime.Compare(cacheDate, DateTime.Today) < 0)
                    Barrel.Current.Add(UPDATE_KEY, false, expireIn: TimeSpan.FromDays(TWENTY_DAYS));
            }
        }

        public void UpdateCacheData()
        {
            if (IsEmpty() || !CacheIsUpToDate)
                CacheDataAsync();
        }

        public Dictionary<string, decimal> GetData()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Cache Is Empty");

            return Barrel.Current.Get<Dictionary<string, decimal>>(DIC_KEY);
        }

        public HashSet<string> GetAcronyms()
        {
            if (!Barrel.Current.Exists(Acronyms_Key))
                return null;

            return Barrel.Current.Get<HashSet<string>>(Acronyms_Key);
        }


    #region Private Methods

        private bool IsEmpty() =>
            !Barrel.Current.Exists(DIC_KEY) && !Barrel.Current.Exists(VERSION_KEY) && !Barrel.Current.Exists(UPDATE_KEY);

        private void InsertData(Dictionary<string, decimal> rates, HashSet<string> currenciesAcronym, string entryDate)
        {
            if (rates == null || rates.Count == 0)
                throw new ArgumentNullException(nameof(rates));

            if (string.IsNullOrEmpty(entryDate))
                throw new ArgumentNullException(nameof(entryDate));

            if (currenciesAcronym.Count == 0)
                throw new ArgumentNullException(nameof(currenciesAcronym));


            if (!string.Equals(Barrel.Current.Get<string>(VERSION_KEY), entryDate))
            {
                Barrel.Current.Add(DIC_KEY,     rates,      expireIn: TimeSpan.FromDays(TWENTY_DAYS));
                Barrel.Current.Add(VERSION_KEY, entryDate,  expireIn: TimeSpan.FromDays(TWENTY_DAYS));
                Barrel.Current.Add(UPDATE_KEY,  true,       expireIn: TimeSpan.FromDays(TWENTY_DAYS));

                if (GetAcronyms() == null || GetAcronyms().Count != currenciesAcronym.Count)
                    Barrel.Current.Add(Acronyms_Key, currenciesAcronym, expireIn: TimeSpan.FromDays(TWENTY_DAYS));
            }
        }

        private string GetContentDate()
        {
            if (!Barrel.Current.Exists(VERSION_KEY))
                return null;

            return Barrel.Current.Get<string>(VERSION_KEY);
        }

        private async void CacheDataAsync()
        {
            JObject responseObject = await CurrencyInfoService.Instance.GetAllCurrenciesRateAsync();

            JToken currencies = responseObject.GetValue("rates");

            Dictionary<string, decimal> currencyRatePair = currencies.ToObject<Dictionary<string, decimal>>();
            HashSet<string> currenciesAcronym = new HashSet<string>(currencyRatePair.Keys);
            string date = responseObject.GetValue("date").ToString();

            InsertData(currencyRatePair, currenciesAcronym, date);
        }

    #endregion
    }
}