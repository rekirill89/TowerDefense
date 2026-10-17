using UnityEngine;

namespace _Project.Scripts
{
    public class EnemyVisual : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private Animator _animator;
        
        private const string IS_MOVE = "IsMove";
        private const string ATTACK = "Attack";

        private void Start()
        {
            _enemy.OnTargetAttack += OnTargetAttackHandler;
        }

        private void OnDestroy()
        {
            _enemy.OnTargetAttack -= OnTargetAttackHandler;
        }

        private void OnTargetAttackHandler()
        {
            _animator.SetTrigger(ATTACK);
        }

        private void Update()
        {
            _animator.SetBool(IS_MOVE, _enemy.IsMove);
        }
    }
}