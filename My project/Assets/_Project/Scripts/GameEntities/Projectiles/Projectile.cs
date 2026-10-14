using System;
using UnityEngine;

namespace _Project.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Projectile : MonoBehaviour
    {
        public event Action<Projectile> OnDestroyed;
        private Action<Projectile> _returnDelegate;
        
        private ProjectileSO _projectileSO;
        private MoveComponent _moveComponent;
        private Rigidbody2D _rb;
        private Transform _target;

        private int _damage;
        private float _speed;
        
        private void Awake()
        {
            _rb =  GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _moveComponent.MoveAndRotateTo(_target.position);
        }
        
        private void LateUpdate()
        {
            if(!_target.gameObject.activeSelf)
                RemoveThisObject();
        }
        
        private void OnDisable()
        {
            transform.position = default;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void Init(
            Vector2 position, 
            Action<Projectile> returnDelegate, 
            ProjectileSO projectileSO, int damage = -1)
        {
            transform.position = position;
            
            _returnDelegate =  returnDelegate;
            _speed = projectileSO.Speed;
            _damage = damage == -1 ? projectileSO.DefaultDamage : damage;
            
            _moveComponent = new MoveComponent(_rb, _speed);
        }

        private void DealDamage(IDamageable target)
        {
            target.TakeDamage(_damage);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                DealDamage(enemy);
                RemoveThisObject();
            }
        }

        private void RemoveThisObject()
        {
            OnDestroyed?.Invoke(this);
            _returnDelegate?.Invoke(this);
        }
    }
}