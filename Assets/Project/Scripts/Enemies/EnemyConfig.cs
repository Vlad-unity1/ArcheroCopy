using Project.Scripts.WeaponModel;
using Project.Scripts.Weapons;
using UnityEngine;

namespace Project.Scripts.Enemies
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObjects/EnemyConfig", order = 59)]
    public class EnemyConfig : ScriptableObject
    {
        public int MaxHealth;
        public EnemyView PrefabEnemy;
        public Weapon<StoneCannonConfig> StartingWeaponConfig;
        public float EXP;

        private void OnValidate()
        {   
            MaxHealth = Mathf.Max(MaxHealth, 1);
        }
    }
}