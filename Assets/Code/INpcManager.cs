using System;

namespace Avangardum.PublexTestTask
{
    public interface INpcManager
    {
        event EventHandler<int> TotalAlliesChanged;
        event EventHandler<int> FollowingAlliesChanged;
        event EventHandler EnemyReachedPlayer;
        event EventHandler<FollowingStatusUpdateArgs> EnemyFollowingStatusUpdate;

        void OnLevelLoaded();
        void Initialize(IFixedUpdateProvider fixedUpdateProvider, INPCConfig config);
    }
}