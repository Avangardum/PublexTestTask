using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Avangardum.PublexTestTask
{
    public class LevelLoader : ILevelLoader
    {
        public event EventHandler LevelLoaded;
        
        private bool _isLoadingLevel;

        public LevelLoader()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!_isLoadingLevel)
            {
                return;
            }

            _isLoadingLevel = false;
            LevelLoaded?.Invoke(this, EventArgs.Empty);
        }

        public void LoadLevel(string id)
        {
            Assert.IsFalse(_isLoadingLevel);
            SceneManager.LoadScene(id);
            _isLoadingLevel = true;
        }
    }
}