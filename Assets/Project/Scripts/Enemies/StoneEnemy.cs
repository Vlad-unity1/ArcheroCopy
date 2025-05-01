using Project.Scripts.Enemies;
using Project.Scripts.HealthInfo;
using Project.Scripts.WeaponModel;
using Project.Scripts.Weapons;
using UnityEngine;

namespace Project.Scripts.Enemy
{
    public class StoneEnemy : EnemyModel
    {
        private readonly MonoBehaviour _coroutineRunner;

        public StoneEnemy(EnemyStoneConfig config, SceneData coroutineRunner, Weapon<StoneCannonConfig> weapon, Health health)
            : base(config, weapon, health, config.EXP)
        {
            _coroutineRunner = coroutineRunner;

            StartAttack();
        }

        public void StopAutoAttack()
        {
            _coroutineRunner.StopAllCoroutines();
        }

        public override void StopAttack()
        {
            _coroutineRunner.StopAllCoroutines();
        }

        public override void StartAttack()
        {
            _coroutineRunner.StartCoroutine(AutoAttack());
        }
    }
}
