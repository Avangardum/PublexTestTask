using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace Avangardum.PublexTestTask
{
    public class PlayerModel : IPlayerModel
    {
        private const string PlayerTag = "Player";
        private const float DestinationDistance = 1f;
        
        private GameObject _playerGO;
        private NavMeshAgent _playerNavMeshAgent;
        private IPlayerConfig _config;
        private Vector3 _movementDirection;
        private IFixedUpdateProvider _fixedUpdateProvider;
        private bool _isLevelLoaded;

        public void SetMovingDirection(Vector3 direction)
        {
            _movementDirection = direction;

            // if (_playerNavMeshAgent == null)
            // {
            //     return;
            // }
            //
            // if (direction != Vector3.zero)
            // {
            //     _playerNavMeshAgent.destination = _playerGO.transform.position + direction.normalized * DestinationDistance;
            // }
            // else
            // {
            //     _playerNavMeshAgent.destination = _playerGO.transform.position;
            // }
        }

        public void OnLevelLoaded()
        {
            _playerGO = GameObject.FindWithTag(PlayerTag);
            Assert.IsNotNull(_playerGO);
            _playerNavMeshAgent = _playerGO.GetComponent<NavMeshAgent>();
            Assert.IsNotNull(_playerNavMeshAgent);
            _playerNavMeshAgent.speed = _config.PlayerSpeed;
            _isLevelLoaded = true;
        }

        public void Initialise(IPlayerConfig config, IFixedUpdateProvider fixedUpdateProvider)
        {
            _config = config;
            _fixedUpdateProvider = fixedUpdateProvider;
            _fixedUpdateProvider.EFixedUpdate += FixedUpdate;
        }

        private void FixedUpdate(object sender, EventArgs e)
        {
            if (!_isLevelLoaded)
            {
                return;
            }
            
            var movement = _movementDirection.normalized * _config.PlayerSpeed * Time.fixedDeltaTime;
            _playerGO.transform.LookAt(_playerGO.transform.position + movement);
            _playerGO.transform.Translate(movement, Space.World);
        }
    }
}