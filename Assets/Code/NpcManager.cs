using System;
using System.Collections.Generic;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    class NpcManager : INPCManager
    {
        private const string PlayerTag = "Player";
        private const string AllyTag = "Ally";
        private const string EnemyTag = "Enemy";
        
        public event EventHandler<int> TotalAlliesChanged;
        public event EventHandler<int> FollowingAlliesChanged;
        public event EventHandler EnemyReachedPlayer;

        private IFixedUpdateProvider _fixedUpdateProvider;
        private INPCConfig _config;
        private List<INPCModel> _allies = new List<INPCModel>();
        private List<INPCModel> _enemies = new List<INPCModel>();
        
        public void OnLevelLoaded()
        {
            Cleanup();

            var playerGO = GameObject.FindWithTag(PlayerTag);
            
            var allyGOs = GameObject.FindGameObjectsWithTag(AllyTag);
            foreach (var allyGO in allyGOs)
            {
                var ally = new NpcModel();
                ally.Initialize(allyGO, playerGO, _fixedUpdateProvider, _config, NPCType.Ally);
                _allies.Add(ally);
            }

            var enemyGOs = GameObject.FindGameObjectsWithTag(EnemyTag);
            foreach (var enemyGO in enemyGOs)
            {
                var enemy = new NpcModel();
                enemy.Initialize(enemyGO, playerGO, _fixedUpdateProvider, _config, NPCType.Enemy);
                _enemies.Add(enemy);
            }
        }

        private void Cleanup()
        {
            _allies.Clear();
            _enemies.Clear();
        }

        public void Initialize(IFixedUpdateProvider fixedUpdateProvider, INPCConfig config)
        {
            _fixedUpdateProvider = fixedUpdateProvider;
            _config = config;
        }
    }
}