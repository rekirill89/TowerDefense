using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts
{
    public class BattleControlService : IDisposable
    {
        public event Action OnBattleFinished;
        public event Action OnBattleStarted;

        private readonly WavesControlService _wavesControlService;
        private readonly Stronghold _stronghold;
        private readonly EnemySpawnService _enemySpawnService;
        private readonly StructureConstructService _structureConstructService;
        private readonly CoinsWallet _coinsWallet;
        private readonly UpgradeStructureService _upgradeStructureService;

        private readonly BattleInfoService _battleInfoService;

        private BattleState _state;

        public BattleControlService(
            WavesControlService wavesControlService, 
            EnemySpawnService enemySpawnService,
            StructureConstructService structureConstructService,
            CoinsWallet coinsWallet,
            UpgradeStructureService upgradeStructureService,
            BattleInfoService battleInfoService,
            Stronghold stronghold)
        {
            _wavesControlService = wavesControlService;
            _enemySpawnService = enemySpawnService;
            _structureConstructService = structureConstructService;
            _coinsWallet = coinsWallet;
            _upgradeStructureService = upgradeStructureService;
            _battleInfoService = battleInfoService;
            _stronghold = stronghold;
        }

        public void Dispose()
        {
            _stronghold.OnDestroyed -= FinishBattle;
        }

        public void Init()
        {
            _stronghold.OnDestroyed += FinishBattle;
            _state = BattleState.NotStarted;
        }

        public void StartBattle()
        {
            Debug.Log("Battle Started");
            _state = BattleState.InProgress;
            _stronghold.StartActing();
            _wavesControlService.RunWaveLoopAsync().Forget();
        }

        public void RestartBattle()
        {
            ClearBattlefield();
            StartBattle();
            OnBattleStarted?.Invoke();
        }

        private void FinishBattle()
        {
            Debug.Log("Battle Finished");
            OnBattleFinished?.Invoke();
            _wavesControlService.StopWave();
            _enemySpawnService.StopAllEnemies();
            _structureConstructService.StopAllTowers();
            
            _state = BattleState.Finished;
        }

        private void ClearBattlefield()
        {
            _stronghold.Reset();
            _enemySpawnService.RemoveAllEnemies();
            _structureConstructService.RemoveAllTowers();
            _coinsWallet.ResetBalance();
            _upgradeStructureService.ResetUpgradeStats();
            _wavesControlService.Reset();
            _battleInfoService.Reset();
        }
    }

    public enum BattleState
    {
        NotStarted,
        InProgress,
        Finished
    }
}