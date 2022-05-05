using System;
using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Avangardum.PublexTestTask
{
    public class FloorButtonMB : MonoBehaviour
    {
        private string PlayerTag = "Player";
        
        public event EventHandler PlayerEntered;
        public event EventHandler PlayerLeft;
        
        [field: SerializeField] public GameObject ConnectedDoor { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                PlayerEntered?.Invoke(this, EventArgs.Empty);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                PlayerLeft?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}