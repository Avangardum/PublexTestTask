using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Avangardum.PublexTestTask
{
    [CreateAssetMenu(menuName = "Config")]
    public class Config : ScriptableObject, IPlayerConfig
    {
        [field: SerializeField] public float PlayerSpeed { get; private set; }
    }
}