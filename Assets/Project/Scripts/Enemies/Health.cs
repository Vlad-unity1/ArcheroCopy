using System;
using UnityEngine;

namespace Project.Scripts.HealthInfo
{
    public class Health
    {
        public event Action OnEntityDeath;
        public event Action<float> OnHealthChanged;

        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        private readonly GameObject _entityObject;

        public Health(float maxHealth, GameObject entityObject)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            _entityObject = entityObject;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Max(CurrentHealth, 0);

            if (CurrentHealth <= 0)
            {
                Die();
            }
            else
            {
                float _currentHealth = CurrentHealth / MaxHealth;
                OnHealthChanged?.Invoke(_currentHealth);
            }
        }

        private void Die()
        {
            UnityEngine.Object.Destroy(_entityObject);
            OnEntityDeath?.Invoke();
        }
    }
}