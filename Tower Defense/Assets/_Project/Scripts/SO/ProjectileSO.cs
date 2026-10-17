using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "Scriptable Objects/ProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    [field: SerializeField] public float Speed {get; private set;}
    [field: FormerlySerializedAs("<Damage>k__BackingField")] [field: SerializeField] public int DefaultDamage {get; private set;}
}
