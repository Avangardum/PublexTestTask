using System;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    class FloorButtonModel : IFloorButtonModel
    {
        public event EventHandler Pressed;
        public event EventHandler Released;

        private GameObject _floorButtonGO;
        private FloorButtonMb _floorButtonMB;

        public void Initialise(GameObject floorButtonGO)
        {
            _floorButtonGO = floorButtonGO;
            _floorButtonMB = _floorButtonGO.GetComponent<FloorButtonMb>();

            _floorButtonMB.PlayerEntered += OnPlayerEntered;
            _floorButtonMB.PlayerLeft += OnPlayerLeft;
        }

        public void Cleanup()
        {
            _floorButtonMB.PlayerEntered -= OnPlayerEntered;
            _floorButtonMB.PlayerLeft -= OnPlayerLeft;
        }

        private void OnPlayerLeft(object sender, EventArgs e)
        {
            Released?.Invoke(this, EventArgs.Empty);
        }

        private void OnPlayerEntered(object sender, EventArgs e)
        {
            Pressed?.Invoke(this, EventArgs.Empty);
        }
    }
}