using System;
using MonkeyCache.SQLite;
using System.Collections.Generic;

namespace Model.Smart_Currency_Converter
{
    // TODO: Make this singleton
    public class Cache
    {
        private const string DIC_KEY = "CurrenciesRateDicKey";
        private const string VERSION_KEY = "RatesDate";
        private const short THIRTY_DAYS = 30;

        // TODO: Handle Parameters
        public void InsertData(Dictionary<string, decimal> rates, string entryDate)
        {
            if (!string.Equals(Barrel.Current.Get<string>(VERSION_KEY), entryDate))
            {
                Barrel.Current.Add(DIC_KEY, rates, expireIn: TimeSpan.FromDays(THIRTY_DAYS));
                Barrel.Current.Add(VERSION_KEY, entryDate, expireIn: TimeSpan.FromDays(THIRTY_DAYS));
            }
        }

        public Dictionary<string, decimal> GetData()
        {
            if (Barrel.Current.IsExpired(DIC_KEY) || Barrel.Current.IsExpired(VERSION_KEY))
                return null;

            return Barrel.Current.Get<Dictionary<string, decimal>>(DIC_KEY);
        }

        public string GetDataVersion()
        {
            if (!Barrel.Current.Exists(VERSION_KEY))
                return null;

            return Barrel.Current.Get<string>(VERSION_KEY);
        }
    }
}