using System.Collections;
using Project.Scripts.HealthInfo;
using Project.Scripts.WeaponModel;
using Project.Scripts.Weapons;
using UnityEngine;

namespace Project.Scripts.Enemies
{
    public class EnemyModel
    {
        public int EXP { get; private set; }
        private Weapon<StoneCannonConfig> CurrentWeapon { get; set; }
        public Health EnemyHealth { get; private set; }

        public EnemyModel(EnemyConfig config, Weapon<StoneCannonConfig> weapon, Health health, int eXP)
        {
            CurrentWeapon = weapon;
            EnemyHealth = health;
            EXP = eXP;
        }

        private void Attack()
        {
            CurrentWeapon.InstantAttack();
        }
        
        public IEnumerator AutoAttack()
        {
            while (true)
            {
                Attack();
                yield return new WaitForSeconds(CurrentWeapon.Config.FireRate);
            }
        }

        public virtual void StopAttack() { }

        public virtual void StartAttack() { }
    }
}