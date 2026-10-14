using System;
using System.Collections.Generic;
using _Project.Scripts.MVP;
using UnityEngine;

namespace _Project.Scripts
{
    public class BattleSceneBootstrap : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private Stronghold _stronghold;
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Archer _archerPrefab;
        [SerializeField] private Cannon _cannonPrefab ;
    
        [Header("Configs")]
        [SerializeField] private RewardsSO _rewardSO;
        [SerializeField] private UpgradeStructureSO _upgradeStrongholdSO;
    
        [Header("Panels")]
        [SerializeField] private InfoPanelView _infoPanelView;
        [SerializeField] private BattleStartPanelView _battleStartPanelView;
        [SerializeField] private ChoseActionPanelView _choseActionPanelView;
        [SerializeField] private UpgradeStructurePanelView _upgradeStructurePanelView;
        [SerializeField] private EndGamePanelView _endGamePanelView;
    
        [Header("Other scene objects")]
        [SerializeField] private GlobalServicesRef globalServicesRef;
        [SerializeField] private ScreenInputHandler _screenInputHandler;

        private readonly DisposableCollector _disposableCollector = new DisposableCollector();

        private BattleControlService _battleControlService;
        private WavesControlService _wavesControlService;
        private EnemySpawnService _enemySpawnService;
        private StructureConstructService _structureConstructService;
        private CoinsWallet _coinsWallet;
        private RewardService _rewardService;
        private UpgradeStructureService _upgradeStructureService;
        private BattleInfoService _battleInfoService;

        private MediatorPresenter _mediatorPresenter;


        void Start()
        {
            _coinsWallet = new CoinsWallet();
            _enemySpawnService = new EnemySpawnService(_enemyPrefab, _stronghold);
            _wavesControlService = new WavesControlService(_enemySpawnService);
            _battleInfoService = new BattleInfoService(_enemySpawnService, _wavesControlService);
            _structureConstructService = new StructureConstructService(_coinsWallet, _archerPrefab, _cannonPrefab);
            _upgradeStructureService = new UpgradeStructureService(_coinsWallet, _stronghold, _upgradeStrongholdSO);
            _battleControlService = new BattleControlService(
                _wavesControlService, 
                _enemySpawnService, 
                _structureConstructService, 
                _coinsWallet,
                _upgradeStructureService,
                _battleInfoService,
                _stronghold);
            _rewardService = new RewardService(
                _battleControlService,
                _battleInfoService,
                _rewardSO,
                _enemySpawnService,
                globalServicesRef.GlobalServices.SaveLoadService,
                _coinsWallet, 
                globalServicesRef.GlobalServices.MetaWallet);

            _mediatorPresenter = new MediatorPresenter(
                _battleControlService,
                _enemySpawnService,
                _wavesControlService,
                _screenInputHandler,
                _structureConstructService,
                _coinsWallet,
                _upgradeStructureService,
                globalServicesRef.GlobalServices.SceneLoaderService,
                _battleStartPanelView,
                _infoPanelView,
                _choseActionPanelView,
                _upgradeStructurePanelView,
                _endGamePanelView);
        
            _battleControlService.Init();
            _rewardService.Init();
            _mediatorPresenter.Init();
            _battleInfoService.Init();
        
            _disposableCollector.Register(new List<IDisposable>()
            {
                _enemySpawnService,
                _wavesControlService,
                _battleInfoService,
                _battleControlService,
                _mediatorPresenter,
                _rewardService,
            });
        }

        private void OnDestroy()
        {
            _disposableCollector.Dispose();
        }
    }
}
