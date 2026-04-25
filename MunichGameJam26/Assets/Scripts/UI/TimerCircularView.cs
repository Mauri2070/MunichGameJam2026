using MGJ.Idle;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MGJ.UI
{
    public class TimerCircularView : MonoBehaviour
    {
        [Header("View Targets")]
        [SerializeField, Tooltip("Will be set to Target Activity Controller Timer if left null.")]
        private Timer.Timer _targetTimer;
        [SerializeField] private IdleActivityController _targetActivityController;

        [Header("UI")]
        [SerializeField] private Image _image;

        private bool _initialized = false;

        private IEnumerator Start()
        {
            yield return null;
            Initialize();
        }

        private void Initialize()
        {
            if (_targetTimer == null)
            {
                if (_targetActivityController == null)
                {
                    Debug.LogWarning($"TimerCircularView {name} has no valid target Timer!");
                    return;
                }

                _targetTimer = _targetActivityController.Timer;
            }

            _targetTimer.OnTimerTicked += Timer_OnTimerTicked;
            SetupImage();

            UpdateView(_targetTimer.TimerProgress);

            _initialized = true;
        }

        private void OnEnable()
        {
            if (_initialized)
            {
                _targetTimer.OnTimerTicked += Timer_OnTimerTicked;
                _targetTimer.OnTimerEnded += Timer_OnTimerEnded;
            }
        }

        private void OnDisable()
        {
            if (_initialized)
            {
                _targetTimer.OnTimerTicked -= Timer_OnTimerTicked;
                _targetTimer.OnTimerEnded -= Timer_OnTimerEnded;
            }
        }

        private void SetupImage()
        {
            _image.type = Image.Type.Filled;
            _image.fillMethod = Image.FillMethod.Radial360;
            _image.fillOrigin = (int)Image.Origin360.Top;
            _image.fillClockwise = true;
        }

        private void Timer_OnTimerTicked(float timerProgress)
        {
            UpdateView(timerProgress);
        }

        private void Timer_OnTimerEnded()
        {
            UpdateView(1);
        }

        private void UpdateView(float timerProgress)
        {
            _image.fillAmount = Mathf.Clamp01(timerProgress);
        }
    }
}
