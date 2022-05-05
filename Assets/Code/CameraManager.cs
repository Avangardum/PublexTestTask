using System;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    class CameraManager : ICameraManager
    {
        private const string CameraTag = "MainCamera";
        private const string PlayerTag = "Player";
        
        private GameObject _camera;
        private GameObject _player;
        private bool _isLevelLoaded;
        private ICameraConfig _config;
        private bool _isResetPositionPending;

        public void OnLevelLoaded()
        {
            _camera = GameObject.FindWithTag(CameraTag);
            _player = GameObject.FindWithTag(PlayerTag);
            _isLevelLoaded = true;
            _isResetPositionPending = true;
        }

        public void Initialize(IFixedUpdateProvider fixedUpdateProvider, ICameraConfig config)
        {
            fixedUpdateProvider.EFixedUpdate += FixedUpdate;
            _config = config;
        }

        private void FixedUpdate(object sender, EventArgs e)
        {
            if (!_isLevelLoaded)
            {
                return;
            }

            var desiredCameraPosition = _player.transform.position + _config.CameraOffset;
            if (_isResetPositionPending)
            {
                _camera.transform.position = desiredCameraPosition;
                _isResetPositionPending = false;
            }
            else
            {
                _camera.transform.position = Vector3.Lerp(_camera.transform.position, desiredCameraPosition, _config.CameraInterpolationSpeed);
            }

            var cameraForwardDirection = _player.transform.position - desiredCameraPosition;
            _camera.transform.LookAt(_camera.transform.position + cameraForwardDirection);
        }
    }
}