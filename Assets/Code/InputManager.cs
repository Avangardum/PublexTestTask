using System;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    class InputManager : IInputManager
    {
        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";
        
        private IUpdateProvider _updateProvider;
        
        public event EventHandler<Vector3> MovementDirectionChanged;
        
        public void Initialize(IUpdateProvider updateProvider)
        {
            _updateProvider = updateProvider;
            _updateProvider.EUpdate += Update;
        }

        private void Update(object sender, EventArgs e)
        {
            var vertical = Input.GetAxis(VerticalAxis);
            var horizontal = Input.GetAxis(HorizontalAxis);
            var movementDirection = new Vector3(horizontal, 0, vertical).normalized;
            MovementDirectionChanged?.Invoke(this, movementDirection);
        }
    }
}