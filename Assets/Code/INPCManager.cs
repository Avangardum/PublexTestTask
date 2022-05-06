using System;

namespace Avangardum.PublexTestTask
{
    public interface INPCManager
    {
        event EventHandler<int> TotalAlliesChanged;
        event EventHandler<int> FollowingAlliesChanged;
        event EventHandler EnemyReachedPlayer;
        
        void OnLevelLoaded();
        void Initialize(IFixedUpdateProvider fixedUpdateProvider, INPCConfig config);
    }
}