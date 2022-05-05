using System;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    public interface IFloorButtonModel
    {
        event EventHandler Pressed;
        event EventHandler Released;
        void Initialise(GameObject floorButtonGO);
        void Cleanup();
    }
}