using System;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    public class FollowingStatusUpdateArgs : EventArgs
    {
        public GameObject CharacterGO;
        public bool IsFollowing;
        public float ProgressPercentage;
        public bool HasReached;
    }
}