using Project.Scripts.BulletModel;
using Project.Scripts.WeaponModel;
using UnityEngine;

namespace Project.Scripts.Weapons
{
    public class Bow : Weapon<BowConfig>
    {
        private readonly Transform _bulletPosition;
        private readonly BulletFactoryPlayer _bulletFactory;

        public Bow(BowConfig bowConfig, Transform bulletPosition, BulletFactoryPlayer bulletFactory)
            : base(bowConfig)
        {
            _bulletPosition = bulletPosition;
            _bulletFactory = bulletFactory;
        }

        public override void InstantAttack()
        {
            var bullet = _bulletFactory.GetBullet(_bulletPosition.position, _bulletPosition.rotation);
            bullet.SetDamage(Config.Damage);
            bullet.Shoot(_bulletPosition.forward, Config.BulletSpeed);

            IncreaseBulletsFired();
        }
    }
}
