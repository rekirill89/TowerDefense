using System;
using System.Collections.Generic;
using _Project.Scripts.MVP;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Scripts
{
    public class StructureConstructService
    {
        public event Action OnStructureConstructed;

        private readonly CoinsWallet _coinsWallet;
        private readonly List<BaseTower> _constructedTowers;
        private readonly Dictionary<StructureEnum, BaseTower> _baseTowersDic;
        
        public StructureConstructService(CoinsWallet coinsWallet, Archer archerPrefab, Cannon cannonPrefab)
        {
            _coinsWallet = coinsWallet;
            _constructedTowers = new List<BaseTower>();
            
            _baseTowersDic = new Dictionary<StructureEnum, BaseTower>()
            {
                { StructureEnum.Archer, archerPrefab },
                { StructureEnum.Cannon, cannonPrefab },
            };
        }

        public void TryConstructStructure(StructureEnum structureEnum, Vector2 pos)
        {
            if(!_coinsWallet.TrySpend(_baseTowersDic[structureEnum].AttackStructureSo.ConstructCost))
                return;
            
            var obj = Object.Instantiate(_baseTowersDic[structureEnum], pos, Quaternion.identity);
            _constructedTowers?.Add(obj);
            OnStructureConstructed?.Invoke();
        }

        public void StopAllTowers()
        {
            foreach (var tower in _constructedTowers)
            {
                tower.StopActing();
            }
        }
        
        public void RemoveAllTowers()
        {
            foreach (var tower in _constructedTowers)
            {
                Object.Destroy(tower.gameObject);
            }
            _constructedTowers.Clear();
        }
    }
}