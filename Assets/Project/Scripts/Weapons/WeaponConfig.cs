using Project.Scripts.BulletModel;
using UnityEngine;

namespace Project.Scripts.Weapons
{
    [CreateAssetMenu(fileName = "New WeaponConfig", menuName = "WeaponConfig", order = 51)]
    public abstract class WeaponConfig : ScriptableObject
    {
        public Bullet BulletPrefab;
        public float BulletSpeed;
        public float FireRate;
        public int Damage;
    }
}