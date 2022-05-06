using System;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    public interface INPCModel
    {
        event EventHandler<FollowingStatusUpdateArgs> FollowingStatusUpdate;
        
        void Initialize(GameObject gameObject, GameObject playerGO, IFixedUpdateProvider fixedUpdateProvider, INPCConfig config, NPCType npcType);
    }
}