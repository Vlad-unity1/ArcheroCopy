using Project.Scripts.WeaponModel;
using Project.Scripts.Weapons;
using UnityEngine;

namespace Project.Scripts.Enemies
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObjects/EnemyConfig", order = 59)]
    public class EnemyConfig : ScriptableObject
    {
        public int MaxHealth;
        public int EXP;
        public EnemyView PrefabEnemy;
        public Weapon<StoneCannonConfig> StartingWeaponConfig;

        private void OnValidate()
        {   
            MaxHealth = Mathf.Max(MaxHealth, 1);
        }
    }
}