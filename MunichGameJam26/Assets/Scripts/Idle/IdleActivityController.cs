using MGJ.Core;
using MGJ.Minigames;
using MGJ.Supporter;
using System;
using UnityEngine;

namespace MGJ.Idle
{
    public class IdleActivityController : MonoBehaviour
    {
        public event Action OnActivityStateChanged;

        [SerializeField] private IdleActivityDefinition _activity;
        public IdleActivityDefinition Activity => _activity;

        private int _currentActivityMilestone;
        private int _nextUpgradeInMilestone;

        private float _currentActivityOutput;
        public float CurrentActivityOutput => _currentActivityOutput;

        private bool _ressourceCollectionAvailable;

        private Timer.Timer _timer;
        public Timer.Timer Timer => _timer;

        private void Awake()
        {
            _currentActivityMilestone = 0;
            _nextUpgradeInMilestone = 0;

            _currentActivityOutput = _activity.BaseRessourceGeneration;
            _ressourceCollectionAvailable = false;

            _timer = new()
            {
                Loop = false
            };
            _timer.SetTimer(_activity.BaseActivityTime);
            _timer.OnTimerEnded += Timer_OnTimerEnded;
        }

        public void UpgradeActivity()
        {
            if (!CanBeUpgraded())
            {
                Debug.Log($"Activity {_activity.ActivityName} has no upgrades left for milestone {_currentActivityMilestone} and step {_nextUpgradeInMilestone}");
                return;
            }

            float upgradeCost = GetCostForNextUpgrade();
            if (MoneyManager.Instance.CanPay(upgradeCost))
            {
                MoneyManager.Instance.Pay(upgradeCost);

                if (_activity.IsMilestoneUpgrade(_currentActivityMilestone, _nextUpgradeInMilestone))
                {
                    MinigameManager.Instance.StartMinigame(this);
                }
                else
                {
                    _currentActivityOutput *= _activity.Milestones[_currentActivityMilestone].RessourceGenerationBoost;
                    _nextUpgradeInMilestone++;
                    OnActivityStateChanged?.Invoke();
                }
            }
        }

        public bool CanBeUpgraded()
        {
            return _activity.HasUpgradesLeft(_currentActivityMilestone, _nextUpgradeInMilestone);
        }

        public float GetCostForNextUpgrade()
        {
            if (!CanBeUpgraded())
            {
                return -1f;
            }

            return _activity.GetCostForUpgrade(_currentActivityMilestone, _nextUpgradeInMilestone);
        }

        public void MilestoneCompleted()
        {
            SupporterManager.Instance.AddSupporter(_activity.Milestones[_currentActivityMilestone].MilestoneSupporter);

            // advance milestone
            _currentActivityMilestone++;
            _nextUpgradeInMilestone = 0;

            // timer setup
            if (_activity.HasUpgradesLeft(_currentActivityMilestone, _nextUpgradeInMilestone))
            {
                _timer.SetTimer(_activity.Milestones[_currentActivityMilestone].MilstoneTime);
            }
            else
            {
                _timer.SetTimer(_activity.FinalActivityTime);
            }

            if (!_timer.Loop)
            {
                _timer.Loop = true;
                _timer.StartTimer();
            }

            OnActivityStateChanged?.Invoke();
        }

        public void InteractWithActivity()
        {
            //Debug.Log($"Interact with activity {_activity.ActivityName}");
            if (_timer.Loop)
            {
                return;
            }

            if (_ressourceCollectionAvailable)
            {
                MoneyManager.Instance.EarnMoney(_currentActivityOutput);
                _ressourceCollectionAvailable = false;
                _timer.StartTimer();
            }
            else if (Mathf.Approximately(_timer.TimerProgress, 0.0f)) // avoid restrting activity on accident
            {
                _timer.StartTimer();
            }
        }

        private void Timer_OnTimerEnded()
        {
            //Debug.Log($"Activity {_activity.ActivityName}: OnTimerEnded");
            if (_timer.Loop)
            {
                MoneyManager.Instance.EarnMoney(_currentActivityOutput);
            }
            else
            {
                _ressourceCollectionAvailable = true;
            }
        }

        public void ChangeMultiplyerBoostStatus(bool applyBoost, float boostMultiplyer)
        {
            if (applyBoost)
            {
                _currentActivityOutput *= boostMultiplyer;
            }
            else
            {
                _currentActivityOutput /= boostMultiplyer;
            }

            OnActivityStateChanged?.Invoke();
        }

        public float TimeWarpAndGetEarnings(float time)
        {
            if (!_timer.Loop)
            {
                _timer.TickTimer(Mathf.Min(time, _timer.Time - _timer.CurrentTime) - Mathf.Epsilon);
                return 0f;
            }

            int wholeTimerCompletions = Mathf.FloorToInt(time / _timer.Time);
            float earnings = wholeTimerCompletions * _currentActivityOutput;

            _timer.TickTimer(time - wholeTimerCompletions * _timer.Time);

            return earnings;
        }
    }
}
