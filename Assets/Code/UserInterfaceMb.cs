using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Avangardum.PublexTestTask
{
    public class UserInterfaceMb : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI AlliesText { get; [UsedImplicitly] private set; }
        [field: SerializeField] public Button RetryButton { get; [UsedImplicitly] private set; }
        [field: SerializeField] public GameObject VictoryWindow { get; [UsedImplicitly] private set; }
        [field: SerializeField] public GameObject DefeatWindow { get; [UsedImplicitly] private set; }
        [field: SerializeField] public GameObject FollowingProgressBarPrefab { get; [UsedImplicitly] private set; }
    }
}