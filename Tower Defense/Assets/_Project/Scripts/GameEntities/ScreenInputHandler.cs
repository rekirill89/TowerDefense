using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts
{
    public class ScreenInputHandler : MonoBehaviour
    {
        public delegate void BuildableClicked(Vector2 pos);
        public event BuildableClicked OnBuildableClicked;
        public event Action OnStrongholdClicked; 
        
        private const string BUILDABLE_LAYER_NAME = "Buildable";
        private const string STRONGHOLD_LAYER_NAME = "Stronghold";

        private LayerMask _layerDefaultMask;
        private LayerMask _layerStrongholdMask;

        private Camera _mainCamera;
        
        private void Start()
        {
            _layerDefaultMask = LayerMask.GetMask(BUILDABLE_LAYER_NAME);
            _layerStrongholdMask = LayerMask.GetMask(STRONGHOLD_LAYER_NAME);
            
            _mainCamera = Camera.main;
        }

        private void Update()
        {

            if (Mouse.current.rightButton.wasPressedThisFrame)
                CheckLayer();
        }
        
        private void CheckLayer()
        {
            if (_mainCamera == null) return;

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 mouseWorldPos = _mainCamera.ScreenToWorldPoint(mousePos);

            Collider2D strongholdHit = Physics2D.OverlapPoint(mouseWorldPos, _layerStrongholdMask);
            if (strongholdHit != null)
            {
                OnStrongholdClicked?.Invoke();
                return; 
            }

            Collider2D buildableHit = Physics2D.OverlapPoint(mouseWorldPos, _layerDefaultMask);
            if (buildableHit != null)
            {
                OnBuildableClicked?.Invoke(mouseWorldPos);
            }
        }
    }
}