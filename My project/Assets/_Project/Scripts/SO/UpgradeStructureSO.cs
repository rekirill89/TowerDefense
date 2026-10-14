using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeStructureSO", menuName = "Scriptable Objects/UpgradeStructureSO")]
public class UpgradeStructureSO : ScriptableObject
{
    [Header("Upgrades values")]
    [field:SerializeField] public UpgradeUnit DamageUpgradeUnit { get; private set; }
    [field:SerializeField] public UpgradeUnit AttackRateUpgradeUnit { get; private set; }
    [field:SerializeField] public UpgradeUnit HealthUpgradeUnit { get; private set; }
}

[Serializable]
public class UpgradeUnit
{
    [field:SerializeField] public float ValuePerLevel { get; private set; }
    [field:SerializeField] public float MultiplierP { get; private set; }
    [field:SerializeField] public int CostPerLevel { get; private set; }
    [field:SerializeField] public float CostMultiplierP { get; private set; }
    
    public void NextLevel()
    {
        ValuePerLevel += ValuePerLevel * (MultiplierP / 100);
        CostPerLevel += (int)(CostPerLevel * (MultiplierP / 100));
    }
}
