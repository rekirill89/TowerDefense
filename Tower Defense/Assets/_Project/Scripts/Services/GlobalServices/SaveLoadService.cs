using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class SaveLoadService
    {
        private const string META_CURRENCY_BALANCE_KEY = "MetaCurrencyBalance";
        
        public void Save(SaveData data)
        {
            PlayerPrefs.SetInt(META_CURRENCY_BALANCE_KEY, data.MetaCurrencyBalance);
        }

        public SaveData Load()
        {
            var metaBalance = PlayerPrefs.GetInt(META_CURRENCY_BALANCE_KEY);
            
            return new SaveData(metaBalance);
        }
    }

    public class SaveData
    {
        public int MetaCurrencyBalance { get; private set; }

        public SaveData(int metaCurrencyBalance)
        {
            MetaCurrencyBalance = metaCurrencyBalance;
        }
    }
}