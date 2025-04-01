using Project.Scripts.BulletFactoryEnemy;
using Project.Scripts.WeaponModel;
using UnityEngine;

namespace Project.Scripts.Weapons
{
    public class StoneCannon : Weapon<StoneCannonConfig>
    {
        private readonly Transform[] _bulletPosition;
        private readonly BulletFactoryEnemies _bulletFactory;

        public StoneCannon(StoneCannonConfig config, Transform[] bulletPosition, BulletFactoryEnemies bulletFactory)
           : base(config)
        {
            _bulletPosition = bulletPosition;
            _bulletFactory = bulletFactory;
        }

        public override void InstantAttack()
        {
            foreach (var bulletPosition in _bulletPosition)
            {
                if (bulletPosition == null) 
                    continue;

                var bullet = _bulletFactory.GetBullet(bulletPosition.position, bulletPosition.rotation);
                bullet.SetDamage(Config.Damage);
                bullet.Shoot(bulletPosition.forward, Config.BulletSpeed);
            }
        }
    }
}