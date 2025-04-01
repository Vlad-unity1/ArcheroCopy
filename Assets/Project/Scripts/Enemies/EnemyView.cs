using Project.Scripts.HealthInfo;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Enemies
{
    public class EnemyView : MonoBehaviour
    {
        public Transform[] WeaponTransform;
        [SerializeField] private Slider _healthBar;
        private EnemyModel _enemyModel;
        private Health _health;

        public void Initialize(EnemyModel enemyModel, Transform[] transform, Health health)
        {
            _enemyModel = enemyModel;
            WeaponTransform = transform;
            _health = health;
            _health.OnHealthChanged += UpdateHealthBar;
            _healthBar.maxValue = 1f;
            _healthBar.value = _health.CurrentHealth / _health.MaxHealth;
        }

        public void TakeDamage(float damage)
        {
            _enemyModel?.EnemyHealth.TakeDamage(damage);
        }

        private void UpdateHealthBar(float currentHealthRatio)
        {
            _healthBar.value = currentHealthRatio;
        }

        private void OnDestroy()
        {
            _health.OnHealthChanged -= UpdateHealthBar;
        }
    }
}
