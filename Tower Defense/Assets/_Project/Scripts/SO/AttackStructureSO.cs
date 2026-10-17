using _Project.Scripts;
using UnityEngine;

[CreateAssetMenu(fileName = "StructureSO", menuName = "Scriptable Objects/StructureSO")]
public class AttackStructureSO : ScriptableObject
{
    [field: SerializeField] public float AttackTimer { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public int ConstructCost { get; private set; }
}
