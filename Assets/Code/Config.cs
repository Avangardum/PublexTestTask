using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Avangardum.PublexTestTask
{
    [CreateAssetMenu(menuName = "Config")]
    public class Config : ScriptableObject, IPlayerConfig, ICameraConfig, INPCConfig
    {
        [field: SerializeField] public float PlayerSpeed { get; private set; }
        [field: SerializeField] public float CameraInterpolationSpeed { get; private set; }
        [field: SerializeField] public Vector3 CameraOffset { get; private set; }
        [field: SerializeField] public float EnemySpeed { get; private set;  }
        [field: SerializeField] public float AllySpeed { get; private set;  }
        [field: SerializeField] public float EnemyUnfollowingDelay { get; private set;  }
        [field: SerializeField] public float EnemyFOV { get; private set;  }
        [field: SerializeField] public float AllyFOV { get; private set;  }
    }
}