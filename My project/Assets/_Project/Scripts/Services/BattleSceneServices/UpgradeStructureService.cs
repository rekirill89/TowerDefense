using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
namespace _Project.Scripts
{
    public class UpgradeStructureService
    {
        public event Action OnStructUpgraded;
        
        private readonly UpgradeStructureSO _upgradeStrongholdSo;
        private readonly Dictionary<UpgradeStructureEnum, Action> _dic;
        private readonly Stronghold _stronghold;
        private readonly CoinsWallet _coinsWallet;

        private UpgradeStructureSO _upgradeStrongholdSoCopy;
        
        public UpgradeStructureService(
            CoinsWallet coinsWallet, 
            Stronghold stronghold, 
            UpgradeStructureSO upgradeStrongholdSo)
        {
            _coinsWallet = coinsWallet;
            _stronghold = stronghold;
            _upgradeStrongholdSo = upgradeStrongholdSo;
            _upgradeStrongholdSoCopy = Object.Instantiate(_upgradeStrongholdSo);
            _dic = new Dictionary<UpgradeStructureEnum, Action>()
            {
                { UpgradeStructureEnum.Damage, () => TryUpgradeDamage() },
                { UpgradeStructureEnum.AttackRate, () => TryUpgradeAttackRate() },
                { UpgradeStructureEnum.Health, () => TryUpgradeHealth() }
            };
        }

        public void UpgradeStruct(UpgradeStructureEnum upgradeEnum)
        {
            _dic[upgradeEnum]();
            OnStructUpgraded?.Invoke();
        }

        public void ResetUpgradeStats()
        {
            Object.Destroy(_upgradeStrongholdSoCopy);
            _upgradeStrongholdSoCopy = Object.Instantiate(_upgradeStrongholdSo);
        }
        
        private void TryUpgradeDamage(int extraDamage = -1)
        {
            if(!_coinsWallet.TrySpend(_upgradeStrongholdSoCopy.DamageUpgradeUnit.CostPerLevel)) 
                return;
            _stronghold.ShootComponent.UpgradeDamage((int)_upgradeStrongholdSoCopy.DamageUpgradeUnit.ValuePerLevel);
            _upgradeStrongholdSoCopy.DamageUpgradeUnit.NextLevel();
        }
        
        private void TryUpgradeAttackRate(int extraRatePercent = -1)
        {
            if(!_coinsWallet.TrySpend(_upgradeStrongholdSoCopy.AttackRateUpgradeUnit.CostPerLevel)) 
                return;
            _stronghold.ShootComponent.UpgradeAttackRate((int)_upgradeStrongholdSoCopy.AttackRateUpgradeUnit.ValuePerLevel);
            _upgradeStrongholdSoCopy.AttackRateUpgradeUnit.NextLevel();
        }
        
        private void TryUpgradeHealth(int extraHealth = -1)
        {
           if(!_coinsWallet.TrySpend(_upgradeStrongholdSoCopy.HealthUpgradeUnit.CostPerLevel)) 
               return;
           _stronghold.HealthComponent.IncreaseHealth((int)_upgradeStrongholdSoCopy.HealthUpgradeUnit.ValuePerLevel);
           _upgradeStrongholdSoCopy.HealthUpgradeUnit.NextLevel();
        }
    }

    public enum UpgradeStructureEnum
    {
        Damage,
        AttackRate,
        Health
    }
}