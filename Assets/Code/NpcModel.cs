using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace Avangardum.PublexTestTask
{
    class NpcModel : INPCModel
    {
        private const string PlayerTag = "Player";
        private const float HasReachedWaypointMaxError = 0.01f;
        
        private enum State
        {
            None = 0,
            Idle = 1,
            Patrolling = 2,
            Following = 3,
        }

        public event EventHandler<FollowingStatusUpdateArgs> FollowingStatusUpdate;

        private GameObject _gameObject;
        private GameObject _playerGO;
        private IFixedUpdateProvider _fixedUpdateProvider;
        private NavMeshAgent _navMeshAgent;
        private State _state;
        private NPCType _npcType;
        private Transform _raycastOrigin;
        private List<Vector3> _patrollingRoute;
        private NPCMB _npcmb;
        private bool _hasPatrollingRoute;
        private INPCConfig _config;
        private float _unfollowingDelayLeft;
        private float _distanceToPlayerWhenBeganFollowing;
        private int _currentWaypointIndex;
        private bool _isPatrollingRouteInverted;
        private LayerMask _raycastLayerMask = LayerMask.GetMask("Default");
        private float _filedOfView;

        private Vector3 CurrentWaypoint => _patrollingRoute[_currentWaypointIndex];

        public void Initialize(GameObject gameObject, GameObject playerGO, IFixedUpdateProvider fixedUpdateProvider, INPCConfig config, NPCType npcType)
        {
            _gameObject = gameObject;
            _playerGO = playerGO;
            _fixedUpdateProvider = fixedUpdateProvider;
            _config = config;
            _npcType = npcType;

            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            Assert.IsNotNull(_navMeshAgent);
            _navMeshAgent.speed = npcType switch
            {
                NPCType.Ally => config.AllySpeed,
                NPCType.Enemy => config.EnemySpeed,
                _ => throw new ArgumentOutOfRangeException(nameof(npcType), npcType, null)
            };
            _filedOfView = npcType switch
            {
                NPCType.Ally => config.AllyFOV,
                NPCType.Enemy => config.EnemyFOV,
                _ => throw new ArgumentOutOfRangeException(nameof(npcType), npcType, null)
            };

            _fixedUpdateProvider.EFixedUpdate += FixedUpdate;

            _npcmb = gameObject.GetComponent<NPCMB>();
            Assert.IsNotNull(_npcmb);
            _raycastOrigin = _npcmb.RaycastOrigin;
            _patrollingRoute = _npcmb.PatrollingRoute.Select(x => x.position).ToList();
            _hasPatrollingRoute = _patrollingRoute.Count >= 2;
        }

        private void FixedUpdate(object sender, EventArgs e)
        {
            UpdateState();
            switch (_state)
            {
                case State.Patrolling:
                    ProcessPatrollingState();
                    break;
                case State.Following:
                    ProcessFollowingState();
                    break;
            }
        }

        private void UpdateState()
        {
            // If there is no current state, set default state
            if (_state == State.None)
            {
                SetDefaultState();
            }
            
            // Detect if the player is visible
            var directionFromRaycastOriginToPlayer = _playerGO.transform.position - _raycastOrigin.position;
            directionFromRaycastOriginToPlayer.y = 0;
            RaycastHit hit;
            var hitExists = Physics.Raycast(_raycastOrigin.position, directionFromRaycastOriginToPlayer, out hit, Mathf.Infinity, _raycastLayerMask);
            var isPlayerVisible = hitExists && hit.collider.CompareTag(PlayerTag);
            
            // If not yet following, there is an extra condition to start following - right orientation of the NPC (looking towards the player)
            var isLookingAtThePlayer = 
                Vector3.Angle(_gameObject.transform.forward, _playerGO.transform.position - _gameObject.transform.position) <= _filedOfView / 2;
            if (_state != State.Following && !isLookingAtThePlayer)
            {
                isPlayerVisible = false;
            }

            // If the player is visible and the npc is not following him, start following
            if (isPlayerVisible && _state != State.Following)
            {
                _state = State.Following;
                _distanceToPlayerWhenBeganFollowing = Vector3.Distance(_gameObject.transform.position, _playerGO.transform.position);
                _unfollowingDelayLeft = _config.EnemyUnfollowingDelay;
            }
            
            // If the player is not visible, the npc is following him, and the npc is enemy,
            // reduce the unfollow delay left, then if it is <= 0, stop following and set the default state
            else if (!isPlayerVisible && _state == State.Following && _npcType == NPCType.Enemy)
            {
                _unfollowingDelayLeft -= Time.fixedDeltaTime;
                if (_unfollowingDelayLeft <= 0)
                {
                    _unfollowingDelayLeft = 0;
                    _navMeshAgent.destination = _gameObject.transform.position;
                    SetDefaultState();
                }
            }
            
            // Sets the state to idle / patrolling
            void SetDefaultState()
            {
                if (_hasPatrollingRoute)
                {
                    _navMeshAgent.SetDestination(CurrentWaypoint);
                    _state = State.Patrolling;
                }
                else
                {
                    _state = State.Idle;
                }
            }
        }
        
        private void ProcessPatrollingState()
        {
            var distanceToCurrentWaypoint = Vector3.Distance(CurrentWaypoint, _gameObject.transform.position);
            bool hasReachedCurrentWaypoint = distanceToCurrentWaypoint < _navMeshAgent.stoppingDistance + HasReachedWaypointMaxError;
            if (hasReachedCurrentWaypoint)
            {
                _currentWaypointIndex += _isPatrollingRouteInverted ? -1 : 1;
                _navMeshAgent.destination = CurrentWaypoint;
                if (_currentWaypointIndex == 0)
                {
                    _isPatrollingRouteInverted = false;
                }
                else if (_currentWaypointIndex == _patrollingRoute.Count - 1)
                {
                    _isPatrollingRouteInverted = true;
                }
            }
        }

        private void ProcessFollowingState()
        {
            _navMeshAgent.destination = _playerGO.transform.position;
        }
    }
}