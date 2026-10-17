using System;
using System.Threading;
using _Project.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    public class BaseTower : MonoBehaviour
    {
        [SerializeField] private EnemyDetector _enemyDetector;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private ProjectileSO _projectileSO;
        [field: SerializeField] public AttackStructureSO AttackStructureSo { get;private set; }
        
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        
        private ShootComponent _shootComponent;
        private Enemy _closestEnemy;
        
        public void StopActing() => _cts.Cancel();
        
        public void Awake()
        {
            _shootComponent = new ShootComponent(
                _projectilePrefab, 
                transform, 
                _closestEnemy, 
                _projectileSO,
                AttackStructureSo);
        }

        private void Start()
        {
            _enemyDetector.Init(AttackStructureSo.AttackRange);
            _shootComponent.AttackAsync(_cts).Forget();
        }

        private void Update()
        {
            CheckTargets();
        }

        private void OnDestroy()
        {
            _cts.Cancel();
        }
        
        private void CheckTargets()
        {
            if (_enemyDetector.Enemies.Count == 0)
            {
                _closestEnemy =  null;
                _shootComponent.SetTarget(_closestEnemy);
                return;
            }
            
            _closestEnemy = _enemyDetector.GetClosestEnemy();
            _shootComponent.SetTarget(_closestEnemy);
        }
    }
}