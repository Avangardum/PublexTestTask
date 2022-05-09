using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;
using Object = UnityEngine.Object;

namespace Avangardum.PublexTestTask
{
    class UserInterface : IUserInterface
    {
        private const float FollowingProgressBarHeight = 2f;

        public event EventHandler RestartClick;

        private UserInterfaceMb _userInterfaceMb;
        private int _foundAllies;
        private int _totalAllies;
        private Dictionary<GameObject, ProgressBar> _followingProgressBars = new Dictionary<GameObject, ProgressBar>();
        private IUpdateProvider _updateProvider;

        public UserInterface()
        {
            _userInterfaceMb = Object.FindObjectOfType<UserInterfaceMb>();
            Assert.IsNotNull(_userInterfaceMb);
        }
        
        public void SetFoundAllies(int value)
        {
            _foundAllies = value;
            UpdateAlliesText();
        }

        public void SetTotalAllies(int value)
        {
            _totalAllies = value;
            UpdateAlliesText();
        }

        public void SetProgressBar(GameObject character, float value)
        {
            ProgressBar progressBar;
            if (_followingProgressBars.ContainsKey(character))
            {
                progressBar = _followingProgressBars[character];
            }
            else
            {
                progressBar = Object.Instantiate(_userInterfaceMb.FollowingProgressBarPrefab, _userInterfaceMb.transform).GetComponent<ProgressBar>();
                _followingProgressBars.Add(character, progressBar);
            }

            progressBar.FillAmount = value;
        }

        public void RemoveProgressBar(GameObject character)
        {
            if (_followingProgressBars.ContainsKey(character))
            {
                Object.Destroy(_followingProgressBars[character].gameObject);
                _followingProgressBars.Remove(character);
            }
        }

        public void ShowDefeatWindow()
        {
            _userInterfaceMb.DefeatWindow.SetActive(true);
        }

        public void ShowVictoryWindow()
        {
            _userInterfaceMb.VictoryWindow.SetActive(true);
        }

        public void HideAllWindows()
        {
            _userInterfaceMb.DefeatWindow.SetActive(false);
            _userInterfaceMb.VictoryWindow.SetActive(false);
        }

        public void OnLevelLoaded()
        {
            foreach (var character in _followingProgressBars.Keys.ToList())
            {
                RemoveProgressBar(character);
            }
        }

        public void Initialize(IUpdateProvider updateProvider)
        {
            _updateProvider = updateProvider;
            _updateProvider.EUpdate += Update;
        }

        private void UpdateAlliesText()
        {
            _userInterfaceMb.AlliesText.text = $"Allies: {_foundAllies}/{_totalAllies}";
        }

        private void Update(object sender, EventArgs eventArgs)
        {
            foreach (var pair in _followingProgressBars)
            {
                var progressBarWorldPosition = pair.Key.transform.position + Vector3.up * FollowingProgressBarHeight;
                Assert.IsNotNull(Camera.main);
                var progressBarScreenPosition = Camera.main.WorldToScreenPoint(progressBarWorldPosition);
                pair.Value.RectTransform.position = progressBarScreenPosition;
            }
        }
    }
}