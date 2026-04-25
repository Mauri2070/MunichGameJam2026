using System.Collections.Generic;
using Unity.VisualScripting;
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
        private List<Timer> _timerToRemove;
        private List<Timer> _timerToAdd;

        private float _currentDelta;

        private void Awake()
        {
            _timer = new HashSet<Timer>();
            _timerToRemove = new List<Timer>();
            _timerToAdd = new List<Timer>();
        }

        private void Update()
        {
            RemoveOldTimers();
            AddNewTimers();

            _currentDelta += Time.deltaTime;
            if (_currentDelta >= _tickFrequency)
            {
                TickTimers(_currentDelta);
                _currentDelta = 0.0f;
            }
        }

        private void RemoveOldTimers()
        {
            foreach (Timer t in _timerToRemove)
            {
                _timer.Remove(t);
            }
            _timerToRemove.Clear();
        }

        private void AddNewTimers()
        {
            _timer.AddRange(_timerToAdd);
            _timerToAdd.Clear();
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
            _timerToAdd.Add(timer);
        }

        public void UnregisterTimer(Timer timer)
        {
            _timerToRemove.Add(timer);
        }
    }
}
