using UnityEngine;

namespace Avangardum.PublexTestTask
{
    public interface ICameraConfig
    {
        public float CameraInterpolationSpeed { get; }
        public Vector3 CameraOffset { get; }
    }
}