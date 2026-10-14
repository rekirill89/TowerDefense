using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.MVP
{
    public class ChoseActionPanelView : BasePanelView
    {
        public event Action<StructureEnum> OnConstructButtonClicked;
        [field: SerializeField] public Canvas LocalCanvas { get; private set; }
        [SerializeField] private CustomButton _constructArcherButton; 
        [SerializeField] private CustomButton _constructCannonButton; 
        
        private Dictionary<StructureEnum, Action> _dic;
        
        private void Start()
        {
            _dic = new Dictionary<StructureEnum, Action>()
            {
                { StructureEnum.Archer, () => ConstructButtonClickedHandler(StructureEnum.Archer) },
                { StructureEnum.Cannon, () => ConstructButtonClickedHandler(StructureEnum.Cannon) },
            };
            
            _constructCannonButton.OnClick += _dic[StructureEnum.Cannon];
            _constructArcherButton.OnClick += _dic[StructureEnum.Archer];
        }

        private void OnDestroy()
        {
            _constructCannonButton.OnClick -= _dic[StructureEnum.Cannon];
            _constructArcherButton.OnClick -= _dic[StructureEnum.Archer];
        }

        private void ConstructButtonClickedHandler(StructureEnum structureEnum)
        {
            Debug.Log(structureEnum);
            OnConstructButtonClicked?.Invoke(structureEnum);
        }
    }
    
    public enum StructureEnum
    {
        Archer,
        Cannon
    }
}