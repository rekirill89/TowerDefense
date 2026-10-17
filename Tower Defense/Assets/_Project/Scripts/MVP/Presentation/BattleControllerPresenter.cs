using System;
using _Project.Scripts.MVP;

namespace _Project.Scripts.MVP
{
    public class BattleControllerPresenter : IDisposable
    {
        private readonly BattleStartPanelView _battleStartPanelView;
        private readonly BattleControlService _battleControlService;
        
        public BattleControllerPresenter(BattleStartPanelView battleStartPanelView, BattleControlService battleControlService)
        {
            _battleStartPanelView = battleStartPanelView;
            _battleControlService = battleControlService;
        }

        public void Dispose()
        {
            _battleStartPanelView.StartButton.OnClick -= _battleControlService.StartBattle;
        }

        public void Init()
        {
            _battleStartPanelView.StartButton.OnClick += OnStartBattleButtonOnClickHandler;
        }

        private void OnStartBattleButtonOnClickHandler()
        {
            _battleControlService.StartBattle();
            _battleStartPanelView.Hide();
        }
    }
}