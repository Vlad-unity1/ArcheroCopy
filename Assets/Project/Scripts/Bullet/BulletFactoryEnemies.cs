using Project.Scripts.BulletModel;
using Project.Scripts.Weapons;
using UnityEngine;

namespace Project.Scripts.BulletFactoryEnemy
{
    public class BulletFactoryEnemies
    {
        private readonly BulletPool _bulletPool;

        public BulletFactoryEnemies(WeaponConfig weaponConfig, int initialPoolSize)
        {
            _bulletPool = new BulletPool(weaponConfig.BulletPrefab, initialPoolSize);
        }

        public Bullet GetBullet(Vector3 position, Quaternion rotation)
        {
            Bullet bullet = _bulletPool.GetBullet();
            bullet.transform.SetPositionAndRotation(position, rotation);
            bullet.gameObject.SetActive(true);
            bullet.OnBulletHit += ReturnToPool;
            return bullet;
        }

        private void ReturnToPool(BulletModel.Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _bulletPool.ReturnBullet(bullet);
            bullet.OnBulletHit -= ReturnToPool;
        }
    }
}
