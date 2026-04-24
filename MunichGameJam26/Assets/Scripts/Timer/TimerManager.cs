using System.Collections.Generic;
using UnityEngine;

namespace MGJ.Timer
{
    public class TimerManager : MonoBehaviour
    {
        private static TimerManager _instance;

        public static TimerManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<TimerManager>();
                    if (_instance == null)
                    {
                        Debug.LogWarning("Could not find TimerManager, creating instance");
                        GameObject go = new GameObject("Timer Manager (created)");
                        _instance = go.AddComponent<TimerManager>();
                    }
                }
                return _instance;
            }
        }

        [SerializeField, Range(0.0f, 1.0f)] private float _tickFrequency = 0.1f;

        private HashSet<Timer> _timer;
        private float _currentDelta;

        private void Awake()
        {
            _timer = new HashSet<Timer>();
        }

        private void Update()
        {
            _currentDelta += Time.deltaTime;
            if (_currentDelta >= _tickFrequency)
            {
                TickTimers(_currentDelta);
                _currentDelta = 0.0f;
            }
        }

        private void TickTimers(float deltaTime)
        {
            foreach (Timer timer in _timer)
            {
                timer.TickTimer(deltaTime);
            }
        }

        public void RegisterTimer(Timer timer)
        {
            _timer.Add(timer);
        }

        public void UnregisterTimer(Timer timer)
        {
            _timer.Remove(timer);
        }
    }
}
