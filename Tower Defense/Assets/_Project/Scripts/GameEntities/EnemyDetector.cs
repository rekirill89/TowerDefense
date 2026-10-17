using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public class EnemyDetector : MonoBehaviour
    {
        [SerializeField] private float _detectRadius = 4;
        
        public List<Enemy> Enemies { get; private set; }
        private CircleCollider2D _circleCollider2D;
        private Enemy _closestEnemy;
        
        private void Awake()
        {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            
            Enemies = new List<Enemy>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                if (!Enemies.Contains(enemy))
                {
                    enemy.OnKilled += OnKilledEnemyHandler;
                    Enemies.Add(enemy);
                }}
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                if (Enemies.Contains(enemy))
                {
                    enemy.OnKilled -= OnKilledEnemyHandler;
                    Enemies.Remove(enemy);
                }
            }
        }

        public void Init(float detectRadius = -1)
        {
            _detectRadius = Mathf.Approximately(detectRadius, -1f) ? _detectRadius :  detectRadius;
            SetDetectRadius(detectRadius);
        }

        public Enemy GetClosestEnemy()
        {
            if (Enemies.Count == 0)
                return null;

            _closestEnemy = Enemies[0];
            float closestDistance = float.MaxValue;
            foreach (var enemy in Enemies)
            {
                var currentDistance = Vector2.Distance(transform.position, enemy.transform.position);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    _closestEnemy = enemy;
                }
            }
            return _closestEnemy;
        }

        private void SetDetectRadius(float detectRadius)
        {
            _detectRadius = detectRadius;
            _circleCollider2D.radius = detectRadius;
        }

        private void OnKilledEnemyHandler(Enemy enemy)
        {
            enemy.OnKilled -= OnKilledEnemyHandler;
            Enemies.Remove(enemy);
        }
    }
}