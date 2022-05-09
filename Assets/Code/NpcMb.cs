using System.Collections.Generic;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    public class NpcMb : MonoBehaviour
    {
        [field: SerializeField] public Transform RaycastOrigin { get; private set; }
        [field: SerializeField] public List<Transform> PatrollingRoute { get; private set; }
    }
}