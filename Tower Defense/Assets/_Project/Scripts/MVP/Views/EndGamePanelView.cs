using UnityEngine;

namespace _Project.Scripts.MVP
{   
    public class EndGamePanelView : BasePanelView
    {
        [field: SerializeField] public CustomButton RestartButton { get; private set; }
        [field: SerializeField] public CustomButton MenuButton { get; private set; } 
    }
}