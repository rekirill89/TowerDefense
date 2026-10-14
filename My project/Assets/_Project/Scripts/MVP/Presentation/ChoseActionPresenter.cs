using System;
using UnityEngine;

namespace _Project.Scripts.MVP
{
    public class ChoseActionPresenter : IDisposable
    {
        private readonly ChoseActionPanelView _choseActionPanelView;
        private readonly ScreenInputHandler _screenInputHandler;
        private readonly StructureConstructService _structureConstructService;
        
        private Vector2 _currentPos =  Vector2.zero;

        public ChoseActionPresenter(
            ChoseActionPanelView choseActionPanelView,
            ScreenInputHandler screenInputHandler,
            StructureConstructService structureConstructService)
        {
            _choseActionPanelView = choseActionPanelView;
            _screenInputHandler = screenInputHandler;
            _structureConstructService = structureConstructService;
        }

        public void Dispose()
        {
            _choseActionPanelView.OnConstructButtonClicked -= ConstructButtonClickedHandler;
            _screenInputHandler.OnBuildableClicked -= BuildableClickedHandler;
            _structureConstructService.OnStructureConstructed -= StructureConstructedHandler;        
        }

        public void Init()
        {
            _choseActionPanelView.OnConstructButtonClicked += ConstructButtonClickedHandler;
            _screenInputHandler.OnBuildableClicked += BuildableClickedHandler;
            _structureConstructService.OnStructureConstructed += StructureConstructedHandler;        
        }

        private void StructureConstructedHandler()
        {
            _choseActionPanelView.Hide();
        }

        private void ConstructButtonClickedHandler(StructureEnum constructEnum)
        {
            _structureConstructService.TryConstructStructure(constructEnum, _currentPos);
        }

        private void BuildableClickedHandler(Vector2 pos)
        {
            _currentPos = pos;
            _choseActionPanelView.LocalCanvas.transform.position = pos; 
            _choseActionPanelView.Show();
        }
    }
}