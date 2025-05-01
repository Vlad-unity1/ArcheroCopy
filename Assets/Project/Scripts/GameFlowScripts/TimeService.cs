using System.Collections.Generic;
using Project.Scripts.Enemies;
using Project.Scripts.PlayerModels;

namespace Project.Scripts.GameFlowScripts
{
    public class TimeService
    {
        private PlayerModel _playerModel;
        private readonly List<EnemyModel> _enemyModels = new();

        public void SetPlayerModel(PlayerModel playerModel)
        {
            _playerModel = playerModel;
        }

        public void SetPEnemyModel(EnemyModel enemyModel)
        {
            if (!_enemyModels.Contains(enemyModel))
                _enemyModels.Add(enemyModel);
        }

        public void Pause()
        {
            foreach (var enemy in _enemyModels)
                enemy.StopAttack();
        }

        public void Continue()
        {
            _playerModel.StartAttack();
            foreach (var enemy in _enemyModels)
                enemy.StartAttack();
        }
    }
}