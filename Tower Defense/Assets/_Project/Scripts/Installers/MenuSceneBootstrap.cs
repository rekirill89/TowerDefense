using System;
using _Project.Scripts.MVP;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    public class MenuSceneBootstrap : MonoBehaviour
    {
        [SerializeField] private GlobalServicesRef _globalServicesRef;
        [SerializeField] private MenuPanelView _menuPanelView;
        
        private readonly DisposableCollector _disposableCollector = new DisposableCollector();
        
        private void Start()
        {
            var menuPanelPresenter = new MenuPanelPresenter(
                _menuPanelView, 
                _globalServicesRef.GlobalServices.MetaWallet,
                _globalServicesRef.GlobalServices.SceneLoaderService);
            
            menuPanelPresenter.Init();
            
            _disposableCollector.Register(menuPanelPresenter);
        }

        private void OnDestroy()
        {
            _disposableCollector.Dispose();
        }
    }
}