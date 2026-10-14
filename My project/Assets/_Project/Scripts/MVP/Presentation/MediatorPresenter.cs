using System;
using System.Collections.Generic;
using _Project.Scripts.MVP;
using _Project.Scripts.MVP;

namespace _Project.Scripts.MVP
{
    public class MediatorPresenter : IDisposable
    {
        private readonly BattleControlService _battleControlService;
        private readonly EnemySpawnService _enemySpawnService;
        private readonly WavesControlService _wavesControlService;
        private readonly ScreenInputHandler _screenInputHandler;
        private readonly StructureConstructService _structureConstructService;
        private readonly CoinsWallet _coinsWallet;
        private readonly UpgradeStructureService _upgradeStructureService;
        private readonly SceneLoaderService _sceneLoaderService;
        private readonly BattleStartPanelView _battleStartPanelView;
        private readonly InfoPanelView _infoPanelView;
        private readonly ChoseActionPanelView _choseActionPanelView;
        private readonly UpgradeStructurePanelView _upgradeStructurePanelView;
        private readonly EndGamePanelView _endGamePanelView;

        private readonly DisposableCollector _disposableCollector = new DisposableCollector();

        public MediatorPresenter(
            BattleControlService battleControlService, 
            EnemySpawnService enemySpawnService,
            WavesControlService wavesControlService,
            ScreenInputHandler screenInputHandler,
            StructureConstructService structureConstructService,
            CoinsWallet coinsWallet,
            UpgradeStructureService upgradeStructureService,
            SceneLoaderService sceneLoaderService,
            BattleStartPanelView battleStartPanelView,
            InfoPanelView infoPanelView,
            ChoseActionPanelView choseActionPanelView,
            UpgradeStructurePanelView upgradeStructurePanelView,
            EndGamePanelView endGamePanelView)
        {
            _battleControlService = battleControlService;
            _enemySpawnService = enemySpawnService;
            _wavesControlService = wavesControlService;
            _screenInputHandler = screenInputHandler;
            _structureConstructService = structureConstructService;
            _coinsWallet = coinsWallet;
            _upgradeStructureService = upgradeStructureService;
            _sceneLoaderService = sceneLoaderService;

            _battleStartPanelView = battleStartPanelView;
            _infoPanelView = infoPanelView;
            _choseActionPanelView =  choseActionPanelView;
            _upgradeStructurePanelView = upgradeStructurePanelView;
            _endGamePanelView = endGamePanelView;
        }

        public void Dispose()
        {
            _disposableCollector.Dispose();
        }

        public void Init()
        {
            var infoPanelPresenter = new InfoPanelPresenter(
                _infoPanelView,
                _battleControlService,
                _enemySpawnService,
                _wavesControlService,
                _coinsWallet);
            var battleControllerPresenter = new BattleControllerPresenter(_battleStartPanelView, _battleControlService);
            var choseActionPresenter = new ChoseActionPresenter(
                _choseActionPanelView, 
                _screenInputHandler, 
                _structureConstructService);
            var upgradeStructurePresenter = new UpgradeStructurePresenter(
                _upgradeStructurePanelView, 
                _upgradeStructureService,
                _screenInputHandler);
            var endGamePresenter = new EndGamePresenter(_endGamePanelView, _battleControlService, _sceneLoaderService);
            
            infoPanelPresenter.Init();
            battleControllerPresenter.Init();
            choseActionPresenter.Init();
            upgradeStructurePresenter.Init();
            endGamePresenter.Init();
            
            _disposableCollector.Register(new List<IDisposable>()
            {
                infoPanelPresenter,
                battleControllerPresenter,
                choseActionPresenter,
                upgradeStructurePresenter,
                endGamePresenter
            });
        }
    }
}