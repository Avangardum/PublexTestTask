using System;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    public class UpdateProvider : MonoBehaviour, IUpdateProvider, IFixedUpdateProvider
    {
        public event EventHandler EUpdate;
        public event EventHandler EFixedUpdate;

        private void Update()
        {
            EUpdate?.Invoke(this, EventArgs.Empty);
        }

        private void FixedUpdate()
        {
            EFixedUpdate?.Invoke(this, EventArgs.Empty);
        }
    }
}