using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Avangardum.PublexTestTask
{
    [CreateAssetMenu(menuName = "Config")]
    public class Config : ScriptableObject, IPlayerConfig, ICameraConfig
    {
        [field: SerializeField] public float PlayerSpeed { get; private set; }
        [field: SerializeField] public float CameraInterpolationSpeed { get; private set; }
        [field: SerializeField] public Vector3 CameraOffset { get; private set; }
    }
}