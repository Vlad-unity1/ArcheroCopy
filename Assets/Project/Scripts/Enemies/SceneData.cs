using Project.Scripts.Players;
using Project.Scripts.Weapons;
using UnityEngine;

namespace Project.Scripts.Enemies
{
    public class SceneData : MonoBehaviour
    {
        public Transform[] SpawnPoints;
        public WeaponFactory _weaponFactory;
        public PlayerMovement PrefabPlayer;

        public float MaxExperience = 10;
    }
}