using MGJ.Idle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGJ.Minigames
{
    public class MinigameManager : MonoBehaviour
    {
        private static MinigameManager _instance;
        public static MinigameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<MinigameManager>();
                    if (_instance == null)
                    {
                        Debug.LogWarning("Could not find MinigameManager. Creating a dummy.");
                        GameObject go = new("Minigame Manager (dummy).");
                        _instance = go.AddComponent<MinigameManager>();
                    }
                }
                return _instance;
            }
        }

        [SerializeField] private List<Minigame> _minigames;

        private MinigameHandler _currentMinigame;
        private IdleActivityController _currentActivity;

        public void StartMinigame(IdleActivityController activityController)
        {
            _currentActivity = activityController;

            Minigame minigame = _minigames[UnityEngine.Random.Range(0, _minigames.Count)];
            _currentMinigame = minigame.Handler;

            _currentMinigame.OnMinigameCompleted += MinigameHandler_OnMinigameCompleted;

            _currentMinigame.OpenMinigame(minigame.MinigameBackground);
        }

        private void MinigameHandler_OnMinigameCompleted(bool success)
        {
            _currentMinigame.OnMinigameCompleted -= MinigameHandler_OnMinigameCompleted;

            if (success)
            {
                _currentActivity.MilestoneCompleted();
            }

            _currentActivity = null;
        }

        [Serializable]
        protected class Minigame
        {
            public MinigameHandler Handler;
            public GameObject MinigameBackground;
        }
    }
}
