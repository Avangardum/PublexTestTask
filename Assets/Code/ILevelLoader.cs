using System;

namespace Avangardum.PublexTestTask
{
    public interface ILevelLoader
    {
        public event EventHandler LevelLoaded;
        public void LoadLevel(string id);
        public void RestartLevel();
    }
}