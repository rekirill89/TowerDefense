using System;
using System.Threading;
using _Project.Scripts;
using _Project.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    public class Stronghold : MonoBehaviour, IDamageable
    {
        //public event Action OnTakeHit;
        public event Action OnDestroyed;
        
        [SerializeField] private EnemyDetector _enemyDetector;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private ProjectileSO _projectileSO;
        [SerializeField] private AttackStructureSO _attackStructureSO;
        public ShootComponent ShootComponent {get; private set; }
        public HealthComponent HealthComponent {  get; private set; }
        
        private Collider2D _collider;
        private Enemy _closestEnemy;

        private int _maxHealth = 200;
        private float _takeDamageTimer = 0f;
        private readonly float _takeDamageDefaultTimer = 0.3f;
        private bool _isActing = false;
        
        private CancellationTokenSource _cts = new CancellationTokenSource();

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            
            HealthComponent = new HealthComponent(_maxHealth);
            ShootComponent = new ShootComponent(
                _projectilePrefab, 
                transform, 
                _closestEnemy, 
                _projectileSO,
                _attackStructureSO);
            
            HealthComponent.OnHealthZero += OnHealthZeroHandler;
        }

        private void Update()
        {
            if (!_isActing)
                return;
            
            CheckTargets();
        }

        private void FixedUpdate()
        {
            _takeDamageTimer = Math.Clamp(_takeDamageTimer - Time.fixedDeltaTime, 0, _takeDamageDefaultTimer);
        }

        private void OnDestroy()
        {
            HealthComponent.OnHealthZero -= OnHealthZeroHandler;
            _cts.Cancel();
        }
        
        public void StartActing()
        {
            _isActing = true;
            ShootComponent.AttackAsync(_cts).Forget();
        }

        public void TakeDamage(int value)
        {
            if (_takeDamageTimer <= 0)
            {
                HealthComponent.ChangeHealthByValue(-value);
                _takeDamageTimer = _takeDamageDefaultTimer;
            }
        }

        public void Reset()
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            
            _collider.enabled = true;
            ResetStats();
        }

        private void ResetStats()
        {
            HealthComponent.ResetHealth();
            ShootComponent.ResetStats();
            ShootComponent.RemoveAllProjectiles();
        }
        
        private void OnHealthZeroHandler()
        {
            _isActing = false;
            _collider.enabled = false;
            _cts.Cancel();
            OnDestroyed?.Invoke();
        }


        private void CheckTargets()
        {
            if (_enemyDetector.Enemies.Count == 0)
            {
                _closestEnemy =  null;
                ShootComponent.SetTarget(_closestEnemy);
                return;
            }
            
            _closestEnemy = _enemyDetector.GetClosestEnemy();
            ShootComponent.SetTarget(_closestEnemy);
        }
    }
}