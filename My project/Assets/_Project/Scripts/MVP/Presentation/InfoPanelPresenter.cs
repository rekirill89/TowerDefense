using System;

namespace _Project.Scripts.MVP
{
    public class InfoPanelPresenter : IDisposable
    {
        private readonly EnemySpawnService _enemySpawnService;
        private readonly WavesControlService _wavesControlService;
        private readonly CoinsWallet _coinsWallet;
        private readonly InfoPanelView _infoPanelView;
        private readonly BattleControlService _battleControlService;

        private int _totalEnemies;
        private int _enemiesLeft;
        
        public InfoPanelPresenter(
            InfoPanelView infoPanelView, 
            BattleControlService battleControlService,
            EnemySpawnService enemySpawnService, 
            WavesControlService wavesControlService, 
            CoinsWallet coinsWallet)
        {
            _enemySpawnService = enemySpawnService;
            _wavesControlService = wavesControlService;
            _coinsWallet = coinsWallet;
            _infoPanelView = infoPanelView;
            _battleControlService = battleControlService;
        }

        public void Dispose()
        {
            _battleControlService.OnBattleFinished -= _infoPanelView.Hide;
            _battleControlService.OnBattleStarted -= _infoPanelView.Show;
            _enemySpawnService.OnEnemyKilled -= OnEnemyKilledHandler;
            _wavesControlService.OnNewWaveStarted -= OnNewWaveStartedHandler;
            _wavesControlService.OnTimerIncremented -= OnTimerIncrementedHandler;
            _coinsWallet.OnCoinsChanged -= OnCoinsChangedHandler;
        }

        public void Init()
        {
            _battleControlService.OnBattleFinished += _infoPanelView.Hide;
            _battleControlService.OnBattleStarted += _infoPanelView.Show;
            _enemySpawnService.OnEnemyKilled += OnEnemyKilledHandler;
            _wavesControlService.OnNewWaveStarted += OnNewWaveStartedHandler;
            _wavesControlService.OnTimerIncremented += OnTimerIncrementedHandler;
            _coinsWallet.OnCoinsChanged += OnCoinsChangedHandler;
            
            _infoPanelView.SetCoinsText(_coinsWallet.CoinsBalance);
        }

        private void OnCoinsChangedHandler(int coins)
        {
            _infoPanelView.SetCoinsText(coins);
        }

        private void OnTimerIncrementedHandler(int timer)
        {
            _infoPanelView.SetWaveTimerText(timer);
        }

        private void OnNewWaveStartedHandler(int index, int totalEnemies)
        {
            _totalEnemies = totalEnemies;
            _enemiesLeft = totalEnemies;
            _infoPanelView.SetWaveIndexText(index);
            _infoPanelView.SetEnemiesCountText(_enemiesLeft, _totalEnemies);
        }

        private void OnEnemyKilledHandler()
        {
            _enemiesLeft--;
            _infoPanelView.SetEnemiesCountText(_enemiesLeft, _totalEnemies);
        }
    }
}