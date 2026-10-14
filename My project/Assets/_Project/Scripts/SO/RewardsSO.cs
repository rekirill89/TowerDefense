using UnityEngine;

namespace _Project.Scripts
{
    [CreateAssetMenu(fileName = "RewardsSO", menuName = "Scriptable Objects/RewardsSO")]
    public class RewardsSO :  ScriptableObject
    {
        [field:SerializeField] public int CoinsPerEnemy {get; private set;}
        [field:SerializeField] public int MetaMoneyPerEnemy {get; private set;}
        [field:SerializeField] public int MetaMoneyPerWave {get; private set;}

    }
}