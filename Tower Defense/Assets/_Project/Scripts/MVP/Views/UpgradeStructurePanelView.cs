using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.MVP
{
    public class UpgradeStructurePanelView : BasePanelView
    {
        public event Action<UpgradeStructureEnum> OnUpgradeStructClicked;
        [SerializeField] private CustomButton _upgradeDamageButton;
        [SerializeField] private CustomButton _upgradeAttackRateButton;
        [SerializeField] private CustomButton _upgradeHealthButton;

        private Dictionary<UpgradeStructureEnum, Action> _dic;

        private void Awake()
        {
            _dic = new Dictionary<UpgradeStructureEnum, Action>()
            {
                { UpgradeStructureEnum.Damage, () => { OnUpgradeStructClicked?.Invoke(UpgradeStructureEnum.Damage); } },
                { UpgradeStructureEnum.AttackRate, () => { OnUpgradeStructClicked?.Invoke(UpgradeStructureEnum.AttackRate); } },
                { UpgradeStructureEnum.Health, () => { OnUpgradeStructClicked?.Invoke(UpgradeStructureEnum.Health); } },
            };
        }

        private void Start()
        {
            _upgradeAttackRateButton.OnClick += _dic[UpgradeStructureEnum.AttackRate];
            _upgradeDamageButton.OnClick += _dic[UpgradeStructureEnum.Damage];
            _upgradeHealthButton.OnClick += _dic[UpgradeStructureEnum.Health];
        }

        private void OnDestroy()
        {
            _upgradeAttackRateButton.OnClick -= _dic[UpgradeStructureEnum.AttackRate];
            _upgradeDamageButton.OnClick -= _dic[UpgradeStructureEnum.Damage];
            _upgradeHealthButton.OnClick -= _dic[UpgradeStructureEnum.Health];
        }
    }
}