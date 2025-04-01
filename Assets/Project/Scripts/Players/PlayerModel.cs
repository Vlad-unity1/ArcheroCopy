using Project.Scripts.Players;
using Project.Scripts.WeaponModel;
using System;
using System.Threading.Tasks;
using Project.Scripts.HealthInfo;
using Project.Scripts.Weapons;

namespace Project.Scripts.PlayerModels
{
    public class PlayerModel
    {
        private const int ATTACK_DELAY = 250;

        public event Action OnAttackStart;
        public event Action OnAttackStop;

        public float CurrentExperience { get; set; }
        public int Speed = 5; // test 
        private bool isAttacking;

        public Health PlayerHealth { get; private set; }
        public Weapon<BowConfig> CurrentWeapon;

        public PlayerMovement PlayerMovement { get; private set; }
        private readonly Joystick _joystick;

        public PlayerModel(Health playerHealth, int speed, Weapon<BowConfig> currentWeapon, PlayerMovement playerMovement, Joystick joystick, float experience)
        {
            PlayerHealth = playerHealth;
            Speed = speed;
            CurrentWeapon = currentWeapon;
            PlayerMovement = playerMovement;
            _joystick = joystick;
            CurrentExperience = experience;
        }

        public void Move()
        {
            PlayerMovement.Move();
        }

        public void StopAttacking()
        {
            isAttacking = false;
        }

        public async void StartAttack()
        {
            if (isAttacking) return;

            isAttacking = true;
            OnAttackStart?.Invoke();

            while (isAttacking)
            {
                CurrentWeapon.InstantAttack();
                
                await Task.Delay(ATTACK_DELAY);

                OnAttackStop?.Invoke();
            }
        }

        public void SetWeapon(Weapon<BowConfig> weapon)
        {
            CurrentWeapon = weapon;
        }
    }
}