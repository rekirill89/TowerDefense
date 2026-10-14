using System;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.MVP
{
    public class MenuPanelView : BasePanelView
    {
        [field:SerializeField] public CustomButton StartButton { get; private set; }
        [field:SerializeField] public TextMeshProUGUI MetaBalance { get; private set; }
        
        public void SetBalanceText(int balance)
        {
            MetaBalance.text = balance.ToString();
        }
    }
}