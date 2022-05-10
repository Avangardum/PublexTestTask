using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Avangardum.PublexTestTask
{
    public class GameManager : IGameManager
    {
        private const string VictoryZoneTag = "Victory Zone";
        
        public event EventHandler Victory;
        public event EventHandler Defeat;

        private TriggerZone _victoryZone;

        public void OnLevelLoaded()
        {
            Time.timeScale = 1;
            if (_victoryZone != null)
            {
                _victoryZone.PlayerEntered -= OnPlayerEnteredVictoryZone;
                _victoryZone = null;
            }

            _victoryZone = GameObject.FindGameObjectWithTag(VictoryZoneTag)?.GetComponent<TriggerZone>();
            Assert.IsNotNull(_victoryZone);
            _victoryZone.PlayerEntered += OnPlayerEnteredVictoryZone;
        }

        private void OnPlayerEnteredVictoryZone(object sender, EventArgs e)
        {
            Victory?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 0;
        }

        public void OnEnemyReachedPlayer()
        {
            Defeat?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 0;;
        }
    }
}