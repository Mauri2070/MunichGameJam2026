using System;
using UnityEngine;

namespace MGJ.Timer
{
    public class Timer
    {
        public event Action OnTimerStarted;
        public event Action OnTimerEnded;
        public event Action<float> OnTimerTicked;

        public bool Loop { get; set; }
        public float Time { get; private set; }
        public float CurrentTime { get; private set; }

        public float TimerProgress
        {
            get
            {
                if (Time <= 0)
                {
                    return 1.0f;
                }

                float _progress = CurrentTime / Time;
                return Mathf.Clamp01(_progress);
            }
        }

        public Timer()
        {
            Time = -1;
            CurrentTime = 0;

        }

        public void TickTimer(float deltaTime)
        {
            CurrentTime += deltaTime;
            OnTimerTicked?.Invoke(TimerProgress);

            if (CurrentTime >= Time)
            {
                CurrentTime -= Time;
                OnTimerEnded?.Invoke();

                if (!Loop)
                {
                    TimerManager.Instance.UnregisterTimer(this);
                    return;
                }
            }
        }

        public void SetTimer(float time, bool start = false)
        {
            Time = time;

            if (start)
            {
                StartTimer();
            }
        }

        public void StartTimer()
        {
            CurrentTime = 0.0f;

            TimerManager.Instance.RegisterTimer(this);

            OnTimerStarted?.Invoke();
        }

        public void StopTimer()
        {
            TimerManager.Instance.UnregisterTimer(this);
        }
    }
}
