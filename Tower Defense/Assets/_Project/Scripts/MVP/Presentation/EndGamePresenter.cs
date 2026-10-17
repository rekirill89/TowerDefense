using System;
using _Project.Scripts.MVP;

namespace _Project.Scripts.MVP
{
    public class EndGamePresenter : IDisposable
    {
        private readonly EndGamePanelView _endGamePanelView;
        private readonly BattleControlService _battleControlService;
        private readonly SceneLoaderService _sceneLoaderService;

        public EndGamePresenter(
            EndGamePanelView endGamePanelView,
            BattleControlService battleControlService, 
            SceneLoaderService sceneLoaderService)
        {
            _endGamePanelView = endGamePanelView;
            _battleControlService = battleControlService;
            _sceneLoaderService = sceneLoaderService;
        }

        public void Dispose()
        {
            _endGamePanelView.RestartButton.OnClick -= RestartButtonOnOnClick;
            _endGamePanelView.MenuButton.OnClick -= _sceneLoaderService.LoadMenuScene;
            
            _battleControlService.OnBattleFinished -= _endGamePanelView.Show;
            _battleControlService.OnBattleStarted -= _endGamePanelView.Hide;
        }

        public void Init()
        {
            _endGamePanelView.RestartButton.OnClick += RestartButtonOnOnClick;
            _endGamePanelView.MenuButton.OnClick += _sceneLoaderService.LoadMenuScene;

            _battleControlService.OnBattleFinished += _endGamePanelView.Show;
            _battleControlService.OnBattleStarted += _endGamePanelView.Hide;
        }

        private void RestartButtonOnOnClick()
        {
            _battleControlService.RestartBattle();
        }
    }
}