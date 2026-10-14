using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class WavesControlService : IDisposable
{
    public delegate void NewWaveStarted(int waveIndex, int totalEnemiesThisWave);
    public event NewWaveStarted OnNewWaveStarted; 
    public event Action<int> OnTimerIncremented;

    private int _totalEnemyAmountPerWave = 0;

    private readonly EnemySpawnService _enemySpawnService;

    private readonly float _maxDirX = 1f;
    private readonly float _maxDirY = 1f;

    private readonly float _spawnInitTimestep = 0.5f; 
    private readonly float _waveMaxTimer = 10f;
    private readonly float _minSpawnDistance = 5;
    private readonly float _maxSpawnDistance = 10;
    private readonly float _breakBetweenWavesTimer = 3f;
    private float _spawnTimestep; 
    private float _waveCurrentTimer = 0;
    private int _waveCurrentIndex = 0;

    private bool _isWaveFinished = false;
    private bool _stopWaveFlag = false;
    
    private CancellationTokenSource _cts = new CancellationTokenSource();

    public WavesControlService(EnemySpawnService enemySpawnService)
    {
        _enemySpawnService = enemySpawnService;
        _spawnTimestep = _spawnInitTimestep;
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    public async UniTask RunWaveLoopAsync()
    {
        while (!_cts.IsCancellationRequested)
        {
            StartWaveAsync().Forget();
            await UniTask.WaitUntil(() => _isWaveFinished, cancellationToken: _cts.Token);
            await UniTask.WaitForSeconds(_breakBetweenWavesTimer, cancellationToken: _cts.Token);
        }    
    }


    public void StopWave()
    {
        _cts.Cancel();
    }

    public void Reset()
    {
        _cts.Cancel();
        _cts = new CancellationTokenSource();
        _spawnTimestep = _spawnInitTimestep;
        _waveCurrentIndex = 0;
        _waveCurrentTimer = 0;
    }

    private async UniTask StartWaveAsync()
    {
        _isWaveFinished = false;
        StartWaveTimerAsync().Forget();

        _waveCurrentIndex++;
        _spawnTimestep -= (_spawnTimestep * (_waveCurrentIndex * 0.1f));
        _totalEnemyAmountPerWave = (int)(_waveMaxTimer / _spawnTimestep);
        OnNewWaveStarted?.Invoke(_waveCurrentIndex, _totalEnemyAmountPerWave);
        
        while (!_cts.IsCancellationRequested && !_stopWaveFlag)
        {
            var dirX = Random.Range(-_maxDirX, _maxDirX);
            var dirY = Random.Range(-_maxDirY, _maxDirY);
            var pos = new Vector2(dirX, dirY).normalized * Random.Range(_minSpawnDistance, _maxSpawnDistance);
            
            _enemySpawnService.SpawnEnemy(pos);

            await UniTask.WaitForSeconds(_spawnTimestep, cancellationToken: _cts.Token);
        }

        _isWaveFinished = true;
    }

    private async UniTask StartWaveTimerAsync()
    {
        _stopWaveFlag = false;
        _waveCurrentTimer = 0;

        int currentSecond = 0;
        int lastReportedSecond = 0;
        
        while (_waveCurrentTimer < _waveMaxTimer && !_cts.IsCancellationRequested)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: _cts.Token);
            _waveCurrentTimer += Time.deltaTime;
            currentSecond = (int)_waveCurrentTimer;
            
            if (currentSecond >= lastReportedSecond)
            {
                OnTimerIncremented?.Invoke(currentSecond);
                lastReportedSecond = currentSecond;
            }
        }
        
        _stopWaveFlag = true;
    }
}
