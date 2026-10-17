using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class HealthComponent
    {
        public event Action OnHealthChanged;
        public event Action OnHealthZero;
    
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }
        private readonly int _maxInitDefaultHealth;

        public HealthComponent(int maxDefaultHealth, int currentHealth = -1)
        {
            _maxInitDefaultHealth = maxDefaultHealth;
            MaxHealth = _maxInitDefaultHealth;

            CurrentHealth = currentHealth == -1 ? MaxHealth : currentHealth;
        }
    
        public void ChangeHealthByValue(int value)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, MaxHealth);
            OnHealthChanged?.Invoke();
       
            if(CurrentHealth <= 0)
                OnHealthZero?.Invoke();
        }

        public void SetHealthOrDefault(int value = -1)
        {
            if(value == -1)
                CurrentHealth = MaxHealth;
            else
                CurrentHealth = value;
        }

        public void IncreaseHealth(int value)
        {
            float temp = (float)CurrentHealth / (float)MaxHealth;
            MaxHealth += value;
            CurrentHealth = (int)(MaxHealth * temp);
            OnHealthChanged?.Invoke();
        }

        public void ResetHealth()
        {
            MaxHealth = _maxInitDefaultHealth;
            CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke();
        }
    }
}
