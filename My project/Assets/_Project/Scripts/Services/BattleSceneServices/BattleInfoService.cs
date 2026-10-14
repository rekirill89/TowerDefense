using System;

namespace _Project.Scripts
{
    public class BattleInfoService : IDisposable
    {
        private readonly EnemySpawnService _enemySpawnService;
        private readonly WavesControlService _wavesControlService;
        private BattleInfoData _battleInfoData;

        public BattleInfoService(EnemySpawnService enemySpawnService, WavesControlService wavesControlService)
        {
            _enemySpawnService = enemySpawnService;
            _wavesControlService = wavesControlService;
            _battleInfoData = new BattleInfoData();
        }

        public void Dispose()
        {
            _enemySpawnService.OnEnemyKilled -= OnEnemyKilledService;
            _wavesControlService.OnNewWaveStarted -= OnNewWaveStarted;
        }

        public void Init()
        {
            _enemySpawnService.OnEnemyKilled += OnEnemyKilledService;
            _wavesControlService.OnNewWaveStarted += OnNewWaveStarted;
        }

        public BattleInfoData GetInfo()
        {
            return _battleInfoData;
        }

        public void Reset()
        {
            _battleInfoData = new BattleInfoData();
        }

        private void OnNewWaveStarted(int _, int __)
        {
            _battleInfoData.IncrementWavesSurvivedAmount();
        }

        private void OnEnemyKilledService()
        {
            _battleInfoData.IncrementEnemyKilledAmount();
        }
    }

    public class BattleInfoData
    {
        public int EnemyKilledAmount {get; private set;}
        public int WavesSurvivedAmount {get; private set;}

        public void IncrementEnemyKilledAmount()
        {
            EnemyKilledAmount++;
        }
        
        public void IncrementWavesSurvivedAmount()
        {
            WavesSurvivedAmount++;
        }
    }
}