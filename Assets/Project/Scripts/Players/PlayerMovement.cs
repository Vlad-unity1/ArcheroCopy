using Project.Scripts.Enemies;
using Project.Scripts.HealthInfo;
using Project.Scripts.Player;
using Project.Scripts.PlayerModels;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Players
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] public Transform weaponTransformPrefab;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private float _enemyDetectionRadius = 50f;
        [SerializeField] private Slider _healthBar;
        private bool _isMoving = false;

        private PlayerInputHandler _inputHandler;
        private PlayerModel _player;
        private Transform _nearestEnemy;
        private Health _health;
        private SceneData _sceneData;

        public void Initialize(PlayerModel player, PlayerInputHandler inputHandler, Health health, SceneData sceneData, int Exp)
        {
            _player = player;
            _inputHandler = inputHandler;
            _health = health;
            _sceneData = sceneData;
            _player.CurrentExperience = Exp;
            _health.OnHealthChanged += UpdateHealthBar;
            _healthBar.maxValue = 1f;
            _healthBar.value = _health.CurrentHealth / _health.MaxHealth;
        }

        public void Move()
        {
            if (_characterController == null || _player == null)
            {
                return;
            }

            Vector3 moveDirection = _inputHandler.GetInputDirection();
            _characterController.Move(moveDirection * (_player.Speed * Time.deltaTime));
            bool isCurrentlyMoving = moveDirection != Vector3.zero;

            if (isCurrentlyMoving && !_isMoving)
            {
                _player.StopAttacking();
            }
            else if (!isCurrentlyMoving && _isMoving)
            {
                _nearestEnemy = FindNearestEnemy();
                if (_nearestEnemy != null)
                {
                    RotateToEnemy();
                    _player.StartAttack();
                }
            }

            _isMoving = isCurrentlyMoving;

            if (isCurrentlyMoving)
            {
                transform.forward = moveDirection;
            }
        }

        private void RotateToEnemy()
        {
            _nearestEnemy = FindNearestEnemy();
            if (_nearestEnemy == null)
            {
                _player.StopAttacking();
                return;
            }

            Vector3 directionToEnemy = (_nearestEnemy.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
            transform.rotation = targetRotation;
        }

        private Transform FindNearestEnemy()
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, _enemyDetectionRadius, _enemyLayer);
            Transform closestEnemy = null;
            float minDistance = Mathf.Infinity;

            foreach (Collider enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }

            return closestEnemy;
        }

        public void TakeDamage(float damage)
        {
            _player?.PlayerHealth.TakeDamage(damage);
        }

        private void UpdateHealthBar(float currentHealthRatio)
        {
            _healthBar.value = currentHealthRatio;
        }

        private void OnDestroy()
        {
            _health.OnHealthChanged -= UpdateHealthBar;
        }

        public void AddExperience(int amount)
        {
            _player.CurrentExperience += amount;

            if (_player.CurrentExperience > _sceneData.MaxExperience)
            {
                _player.CurrentExperience = _sceneData.MaxExperience;
            }
        }
    }
}