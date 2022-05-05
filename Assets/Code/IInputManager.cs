using System;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    public interface IInputManager
    {
        event EventHandler<Vector3> MovementDirectionChanged;
        void Initialize(IUpdateProvider updateProvider);
    }
}