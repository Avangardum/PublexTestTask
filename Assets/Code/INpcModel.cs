using System;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    public interface INpcModel
    {
        event EventHandler<FollowingStatusUpdateArgs> FollowingStatusUpdate;
        
        void Initialize(GameObject gameObject, GameObject playerGO, IFixedUpdateProvider fixedUpdateProvider, INPCConfig config, NpcType npcType);
    }
}