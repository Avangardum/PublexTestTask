using System;

namespace Avangardum.PublexTestTask
{
    public interface IGameManager
    {
        event EventHandler Victory;
        event EventHandler Defeat;

        void OnLevelLoaded();
        void OnEnemyReachedPlayer();
    }
}