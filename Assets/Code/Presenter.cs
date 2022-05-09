using System;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    public class Presenter
    {
        public struct Dependencies
        {
            public IPlayerModel PlayerModel;
            public ILevelLoader LevelLoader;
            public IInputManager InputManager;
            public ICameraManager CameraManager;
            public IFloorButtonsAndDoorsManager FloorButtonsAndDoorsManager;
            public INpcManager NpcManager;
            public IUserInterface UserInterface;
        }

        private Dependencies _dependencies;
        
        public Presenter(Dependencies dependencies)
        {
            _dependencies = dependencies;

            _dependencies.LevelLoader.LevelLoaded += OnLevelLoaded;
            _dependencies.InputManager.MovementDirectionChanged += InputManagerOnMovementDirectionChanged;

            _dependencies.NpcManager.FollowingAlliesChanged += OnFollowingAlliesChanged;
            _dependencies.NpcManager.TotalAlliesChanged += OnTotalAlliesChanged;
            _dependencies.NpcManager.EnemyReachedPlayer += OnEnemyReachedPlayer;
            _dependencies.NpcManager.EnemyFollowingStatusUpdate += OnEnemyFollowingStatusUpdate;
        }

        private void InputManagerOnMovementDirectionChanged(object sender, Vector3 value)
        {
            _dependencies.PlayerModel.SetMovingDirection(value);
        }

        private void OnLevelLoaded(object sender, EventArgs e)
        {
            _dependencies.PlayerModel.OnLevelLoaded();
            _dependencies.CameraManager.OnLevelLoaded();
            _dependencies.FloorButtonsAndDoorsManager.OnLevelLoaded();
            _dependencies.NpcManager.OnLevelLoaded();
            _dependencies.UserInterface.OnLevelLoaded();
        }

        #region NPC Manager Callbacks

        private void OnFollowingAlliesChanged(object sender, int value)
        {
            _dependencies.UserInterface.SetFoundAllies(value);
        }

        private void OnTotalAlliesChanged(object sender, int value)
        {
            _dependencies.UserInterface.SetTotalAllies(value);
        }

        private void OnEnemyReachedPlayer(object sender, EventArgs e)
        {
            
        }

        private void OnEnemyFollowingStatusUpdate(object sender, FollowingStatusUpdateArgs args)
        {
            if (args.IsFollowing)
            {
                _dependencies.UserInterface.SetProgressBar(args.CharacterGO ,args.ProgressPercentage);
            }
            else
            {
                _dependencies.UserInterface.RemoveProgressBar(args.CharacterGO);
            }
        }

        #endregion
    }
}