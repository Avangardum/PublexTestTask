using System;
using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Avangardum.PublexTestTask
{
    public class FloorButtonMb : TriggerZone
    {
        [field: SerializeField] public GameObject ConnectedDoor { get; private set; }
    }
}