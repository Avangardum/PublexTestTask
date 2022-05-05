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
        }

        private Dependencies _dependencies;
        
        public Presenter(Dependencies dependencies)
        {
            _dependencies = dependencies;

            _dependencies.LevelLoader.LevelLoaded += OnLevelLoaded;
            _dependencies.InputManager.MovementDirectionChanged += InputManagerOnMovementDirectionChanged;
        }

        private void InputManagerOnMovementDirectionChanged(object sender, Vector3 value)
        {
            _dependencies.PlayerModel.SetMovingDirection(value);
        }

        private void OnLevelLoaded(object sender, EventArgs e)
        {
            _dependencies.PlayerModel.OnLevelLoaded();
        }
    }
}