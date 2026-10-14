using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class RewardService : IDisposable
    {
        public delegate void RewardGained(RewardEnum rewardEnum, int amount);
        public event RewardGained OnRewardGained;

        private readonly BattleControlService _battleControlService;
        private readonly BattleInfoService _battleInfoService;
        private readonly EnemySpawnService _enemySpawnService;
        private readonly SaveLoadService _saveLoadService;
        private readonly CoinsWallet _coinsWallet;
        private readonly MetaCurrencyWallet _metaWallet;
        private RewardsSO _rewardSO;

        public RewardService(
            BattleControlService battleControlService,
            BattleInfoService battleInfoService,
            RewardsSO rewardsSO,
            EnemySpawnService enemySpawnService, 
            SaveLoadService saveLoadService,
            CoinsWallet coinsWallet,
            MetaCurrencyWallet metaWallet)
        {
            _battleControlService = battleControlService;
            _battleInfoService = battleInfoService;
            _enemySpawnService = enemySpawnService;
            _saveLoadService = saveLoadService;
            _coinsWallet = coinsWallet;
            _metaWallet = metaWallet;
            _rewardSO = rewardsSO;
        }

        public void Dispose()
        {
            _enemySpawnService.OnEnemySpawned -= OnEnemySpawnedHandler; 
            _battleControlService.OnBattleFinished -= OnBattleFinishedHandler;
        }

        public void Init()
        {
            _enemySpawnService.OnEnemySpawned += OnEnemySpawnedHandler;
            _battleControlService.OnBattleFinished += OnBattleFinishedHandler;
        }

        private void OnBattleFinishedHandler()
        {
            var info = _battleInfoService.GetInfo();
            var value = _rewardSO.MetaMoneyPerWave * info.WavesSurvivedAmount +
                        _rewardSO.MetaMoneyPerEnemy * info.EnemyKilledAmount;
            _metaWallet.Collect(value);
            _saveLoadService.Save(new SaveData(_metaWallet.CurrentBalance));
            Debug.Log("Collected: " + value + ", current balance: " + _metaWallet.CurrentBalance);
        }

        private void OnEnemySpawnedHandler(Enemy enemy)
        {
            enemy.OnKilled += OnEnemyKilledHandler;
        }

        private void OnEnemyKilledHandler(Enemy enemy)
        {
            _coinsWallet.Collect(_rewardSO.CoinsPerEnemy);
            enemy.OnKilled -= OnEnemyKilledHandler;
            OnRewardGained?.Invoke(RewardEnum.Coins, _rewardSO.CoinsPerEnemy);
        }
    }

    public enum RewardEnum
    {
        Coins,
        Crystals
    }
}