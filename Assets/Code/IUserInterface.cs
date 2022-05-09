using System;
using UnityEngine;

namespace Avangardum.PublexTestTask
{
    public interface IUserInterface
    {
        event EventHandler RestartClick;
        
        void SetFoundAllies(int value);
        void SetTotalAllies(int value);
        void SetProgressBar(GameObject character, float value);
        void RemoveProgressBar(GameObject character);
        void ShowDefeatWindow();
        void ShowVictoryWindow();
        void HideAllWindows();
        void OnLevelLoaded();
        void Initialize(IUpdateProvider updateProvider);
    }
}