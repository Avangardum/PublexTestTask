using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace Avangardum.PublexTestTask
{
    class NpcModel : INpcModel
    {
        private const string PlayerTag = "Player";
        private const float HasReachedWaypointMaxError = 0.01f;
        private const float TargetDistanceWhenFollowing = 0.8f;
        
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
        private NpcType _npcType;
        private Transform _raycastOrigin;
        private List<Vector3> _patrollingRoute;
        private NpcMb _npcMb;
        private bool _hasPatrollingRoute;
        private INPCConfig _config;
        private float _unfollowingDelayLeft;
        private float _distanceToPlayerWhenBeganFollowing;
        private int _currentWaypointIndex;
        private bool _isPatrollingRouteInverted;
        private LayerMask _raycastLayerMask = LayerMask.GetMask("Default");
        private float _filedOfView;

        private Vector3 CurrentWaypoint => _patrollingRoute[_currentWaypointIndex];
        private float DistanceToPlayer => Vector3.Distance(_gameObject.transform.position, _playerGO.transform.position);

        public void Initialize(GameObject gameObject, GameObject playerGO, IFixedUpdateProvider fixedUpdateProvider, INPCConfig config, NpcType npcType)
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
                NpcType.Ally => config.AllySpeed,
                NpcType.Enemy => config.EnemySpeed,
                _ => throw new ArgumentOutOfRangeException(nameof(npcType), npcType, null)
            };
            _filedOfView = npcType switch
            {
                NpcType.Ally => config.AllyFOV,
                NpcType.Enemy => config.EnemyFOV,
                _ => throw new ArgumentOutOfRangeException(nameof(npcType), npcType, null)
            };

            _fixedUpdateProvider.EFixedUpdate += FixedUpdate;

            _npcMb = gameObject.GetComponent<NpcMb>();
            Assert.IsNotNull(_npcMb);
            _raycastOrigin = _npcMb.RaycastOrigin;
            _patrollingRoute = _npcMb.PatrollingRoute.Select(x => x.position).ToList();
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
                _distanceToPlayerWhenBeganFollowing = DistanceToPlayer;
                _unfollowingDelayLeft = _config.EnemyUnfollowingDelay;
            }
            
            // If the player is not visible, the npc is following him, and the npc is enemy,
            // reduce the unfollow delay left, then if it is <= 0, stop following and set the default state
            else if (!isPlayerVisible && _state == State.Following && _npcType == NpcType.Enemy)
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
                InvokeFollowingStatusUpdate();
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
            InvokeFollowingStatusUpdate();
        }

        private void InvokeFollowingStatusUpdate()
        {
            FollowingStatusUpdate?.Invoke(this, new FollowingStatusUpdateArgs
            {
                CharacterGO = _gameObject,
                HasReached = false, 
                IsFollowing = _state == State.Following, 
                ProgressPercentage = 1 - Mathf.Clamp01((DistanceToPlayer - TargetDistanceWhenFollowing) / _distanceToPlayerWhenBeganFollowing),
            });
        }
    }
}