using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class CoinsWallet
    {
        public delegate void CoinsChanged(int coins);
        public event CoinsChanged OnCoinsChanged;
        public int CoinsBalance {get; private set;}

        private readonly int _initCoinsBalance = 0;

        public CoinsWallet()
        {
            Collect(_initCoinsBalance);
        }
        
        public void Collect(int coins)
        {
            if(coins < 0 )
            {
                Debug.LogWarning($"[Economy] Attempted to collect invalid amount: {coins}");
                return;
            }
            CoinsBalance += coins;
            OnCoinsChanged?.Invoke(CoinsBalance);
        }

        public bool TrySpend(int cost)
        {
            if(cost < 0 )
            {
                Debug.LogWarning($"[Economy] Attempted to spend invalid amount: {cost}");
                return false;
            }

            if (cost > CoinsBalance)
            {
                Debug.Log($"[Economy] Insufficient money, balance:{CoinsBalance}/cost:{cost}");
                return false;
            }
            CoinsBalance -= cost;
            OnCoinsChanged?.Invoke(CoinsBalance);
            return true;
        }
        
        public void ResetBalance()
        {
            CoinsBalance = _initCoinsBalance;
        }
    }
}