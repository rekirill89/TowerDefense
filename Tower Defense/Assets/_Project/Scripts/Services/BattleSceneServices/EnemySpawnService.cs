using System;
using System.Collections.Generic;
using _Project.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

public class EnemySpawnService : IDisposable
{
    public delegate void EnemiesCountChanged(int count);
    public delegate void EnemySpawned(Enemy enemy);
    public event EnemiesCountChanged OnEnemiesCountChanged;
    public event Action OnEnemyKilled;
    public event EnemySpawned OnEnemySpawned;
    
    private readonly List<Enemy> _enemies;
    private readonly Stronghold _stronghold;
    private readonly ObjectPool<Enemy> _objectPool;

    public EnemySpawnService(Enemy enemyPrefab, Stronghold stronghold)
    {
        _stronghold = stronghold;
        _objectPool = new ObjectPool<Enemy>(enemyPrefab);

        _enemies = new List<Enemy>();
    }

    public void Dispose()
    {
        foreach (var enemy in _enemies)
        {
            enemy.OnKilled -= OnEnemyKilledHandler;
        }
    }

    public void SpawnEnemy(Vector2 position = default)
    {
        var enemy = _objectPool.Get(); 
        enemy.Init(position, _objectPool.Return);
        enemy.SetTarget(_stronghold.transform.position);
        
        _enemies.Add(enemy);
        OnEnemiesCountChanged?.Invoke(_enemies.Count);
        OnEnemySpawned?.Invoke(enemy);
        enemy.OnKilled += OnEnemyKilledHandler;
    }

    public void StopAllEnemies()
    {
        foreach (var enemy in _enemies)
        {
            enemy.StopActing();
        }
    }

    public void RemoveAllEnemies()
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            _enemies[i].OnKilled -= OnEnemyKilledHandler;
            _enemies[i].RemoveObject();
        }
        _enemies.Clear();
        OnEnemiesCountChanged?.Invoke(_enemies.Count);    
    }

    private void OnEnemyKilledHandler(Enemy enemy)
    {
        enemy.OnKilled -= OnEnemyKilledHandler;
        if(_enemies.Contains(enemy))
            _enemies.Remove(enemy);

        OnEnemyKilled?.Invoke();
        OnEnemiesCountChanged?.Invoke(_enemies.Count);
    }
}
