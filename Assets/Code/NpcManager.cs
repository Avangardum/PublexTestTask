using System;
using System.Collections.Generic;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    class NpcManager : INpcManager
    {
        private const string PlayerTag = "Player";
        private const string AllyTag = "Ally";
        private const string EnemyTag = "Enemy";

        public event EventHandler<int> TotalAlliesChanged;
        public event EventHandler<int> FollowingAlliesChanged;
        public event EventHandler EnemyReachedPlayer;
        public event EventHandler<FollowingStatusUpdateArgs> EnemyFollowingStatusUpdate;


        private IFixedUpdateProvider _fixedUpdateProvider;
        private INPCConfig _config;
        private List<INpcModel> _allies = new List<INpcModel>();
        private List<INpcModel> _enemies = new List<INpcModel>();
        private List<INpcModel> _followingAllies = new List<INpcModel>();
        
        public void OnLevelLoaded()
        {
            Cleanup();

            var playerGO = GameObject.FindWithTag(PlayerTag);
            
            var allyGOs = GameObject.FindGameObjectsWithTag(AllyTag);
            foreach (var allyGO in allyGOs)
            {
                var ally = new NpcModel();
                ally.Initialize(allyGO, playerGO, _fixedUpdateProvider, _config, NpcType.Ally);
                _allies.Add(ally);
                ally.FollowingStatusUpdate += OnAllyFollowingStatusUpdate;
            }
            TotalAlliesChanged?.Invoke(this, _allies.Count);

            var enemyGOs = GameObject.FindGameObjectsWithTag(EnemyTag);
            foreach (var enemyGO in enemyGOs)
            {
                var enemy = new NpcModel();
                enemy.Initialize(enemyGO, playerGO, _fixedUpdateProvider, _config, NpcType.Enemy);
                _enemies.Add(enemy);
                enemy.FollowingStatusUpdate += OnEnemyFollowingStatusUpdate;
            }
        }

        private void OnEnemyFollowingStatusUpdate(object sender, FollowingStatusUpdateArgs args)
        {
            EnemyFollowingStatusUpdate?.Invoke(this, args);
            if (args.HasReached)
            {
                EnemyReachedPlayer?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnAllyFollowingStatusUpdate(object sender, FollowingStatusUpdateArgs args)
        {
            var ally = (INpcModel) sender;
            if (args.IsFollowing && !_followingAllies.Contains(ally))
            {
                _followingAllies.Add(ally);
                FollowingAlliesChanged?.Invoke(this, _followingAllies.Count);
            }
        }

        private void Cleanup()
        {
            foreach (var ally in _allies)
            {
                ally.FollowingStatusUpdate -= OnAllyFollowingStatusUpdate;
                ally.Cleanup();
            }
            _allies.Clear();
            foreach (var enemy in _enemies)
            {
                enemy.FollowingStatusUpdate -= OnEnemyFollowingStatusUpdate;
                enemy.Cleanup();
            }
            _enemies.Clear();
            _followingAllies.Clear();
            FollowingAlliesChanged?.Invoke(this, 0);
        }

        public void Initialize(IFixedUpdateProvider fixedUpdateProvider, INPCConfig config)
        {
            _fixedUpdateProvider = fixedUpdateProvider;
            _config = config;
        }
    }
}