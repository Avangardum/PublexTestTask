using System;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    public class TriggerZone : MonoBehaviour
    {
        private string PlayerTag = "Player";
        
        public event EventHandler PlayerEntered;
        public event EventHandler PlayerLeft;

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