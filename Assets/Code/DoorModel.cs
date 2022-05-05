using System;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    class DoorModel : IDoorModel
    {
        private GameObject _doorGO;
        private bool _isOpen;

        public void Open()
        {
            _doorGO.SetActive(false);
            _isOpen = true;
        }

        public void Close()
        {
            _doorGO.SetActive(true);
            _isOpen = false;
        }

        public void Switch()
        {
            if (_isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public void Initialize(GameObject doorGO)
        {
            _doorGO = doorGO;
        }
    }
}