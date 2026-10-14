using _Project.Scripts.MVP;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.MVP
{
    public class InfoPanelView : BasePanelView
    {
        [SerializeField] private TextMeshProUGUI _enemiesCountText;
        [SerializeField] private TextMeshProUGUI _waveIndexText;
        [SerializeField] private TextMeshProUGUI _waveTimerText;
        [SerializeField] private TextMeshProUGUI _coinsText;

        public void SetEnemiesCountText(int enemiesLeft, int totalEnemies)
        {
            _enemiesCountText.text = $"{enemiesLeft}/{totalEnemies}";
        }
        
        public void SetWaveIndexText(int index)
        {
            _waveIndexText.text = $"Wave {index.ToString()}";
        }
        
        public void SetWaveTimerText(float timer)
        {
            _waveTimerText.text = timer.ToString("0.0") + "s";
        }

        public void SetCoinsText(int coins)
        {
            _coinsText.text = "Coins: " + coins;
        }
    }
}