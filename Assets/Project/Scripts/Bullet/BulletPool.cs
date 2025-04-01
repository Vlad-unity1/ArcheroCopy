using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.BulletModel
{
    public class BulletPool
    {
        private readonly List<Bullet> _pool = new();
        private readonly Bullet _bulletPrefab;

        public BulletPool(Bullet bulletPrefab, int initialSize)
        {
            _bulletPrefab = bulletPrefab;

            for (int i = 0; i < initialSize; i++)
            {
                Bullet bullet = CreateBullet();
                _pool.Add(bullet);
            }
        }

        public Bullet GetBullet()
        {
            foreach (var bullet in _pool)
            {
                if (!bullet.gameObject.activeInHierarchy)
                {
                    return bullet;
                }
            }

            Bullet newBullet = CreateBullet();
            _pool.Add(newBullet);
            return newBullet;
        }

        public void ReturnBullet(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            bullet.OnBulletHit -= ReturnBullet;
        }

        private Bullet CreateBullet()
        {
            Bullet bullet = Object.Instantiate(_bulletPrefab);
            bullet.gameObject.SetActive(false);
            bullet.OnBulletHit += ReturnBullet;
            return bullet;
        }
    }
}
