using MGJ.Core;
using MGJ.Idle;
using System.Collections.Generic;
using UnityEngine;

namespace MGJ.Boosts
{
    public class BoostsManager : MonoBehaviour
    {
        [Header("Multiplier Boost Settings")]
        [SerializeField, Range(30f, 6000f)] private float _multiplierBoostTime = 60f;
        [SerializeField, Range(1f, 10f)] private float _multiplierBoostFactor = 2f;

        [Header("Time Warp Boost Settings")]
        [SerializeField, Range(30f, 6000f)] private float _timeWarpDuration = 300f;

        [SerializeField] private List<IdleActivityController> _activityConrollers;

        private Timer.Timer _multiplierBoostTimer;
        public Timer.Timer MultiplierBoostTimer => _multiplierBoostTimer;

        private void Awake()
        {
            _multiplierBoostTimer = new()
            {
                Loop = false
            };
            _multiplierBoostTimer.SetTimer(_multiplierBoostTime);

            _multiplierBoostTimer.OnTimerEnded += Timer_OnTimerEnded;
        }

        public void ApplyBoost(BoostType boostType)
        {
            switch (boostType)
            {
                case BoostType.Earnings:
                    ApplyMultiplyerBoost();
                    break;
                case BoostType.TimeWarp:
                    ApplyTimeWarpBoost();
                    break;
                case BoostType.Supporter:
                    ApplySupporterBoost();
                    break;
            }
        }

        public void ApplyMultiplyerBoost()
        {
            if (Mathf.Approximately(_multiplierBoostTimer.CurrentTime, 0.0f))
            {
                // only apply multiplier, if timer is not already active
                foreach (IdleActivityController activityController in _activityConrollers)
                {
                    activityController.ChangeMultiplyerBoostStatus(true, _multiplierBoostFactor);
                }
            }
            _multiplierBoostTimer.StartTimer();
        }

        private void Timer_OnTimerEnded()
        {
            foreach (IdleActivityController activityController in _activityConrollers)
            {
                activityController.ChangeMultiplyerBoostStatus(false, _multiplierBoostFactor);
            }
            _multiplierBoostTimer.ResetCurrentTime();
        }

        public void ApplyTimeWarpBoost()
        {
            float totalEarnings = 0f;
            foreach (IdleActivityController activityController in _activityConrollers)
            {
                totalEarnings += activityController.TimeWarpAndGetEarnings(_timeWarpDuration);
            }

            MoneyManager.Instance.EarnMoney(totalEarnings);
        }

        public void ApplySupporterBoost()
        {
            // TODO
        }
    }
}
