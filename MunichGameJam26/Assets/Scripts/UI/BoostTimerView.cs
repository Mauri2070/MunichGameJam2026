using MGJ.Boosts;
using UnityEngine;
using UnityEngine.UI;

namespace MGJ.UI
{
    public class BoostTimerView : MonoBehaviour
    {
        [SerializeField] private BoostsManager _boostManager;
        [SerializeField] private Image _displayImage;

        private void Awake()
        {
            _displayImage.fillAmount = 0;
        }

        private void OnEnable()
        {
            _boostManager.MultiplierBoostTimer.OnTimerTicked += Timer_OnTimerTicked;
        }

        private void OnDisable()
        {
            _boostManager.MultiplierBoostTimer.OnTimerTicked -= Timer_OnTimerTicked;
        }

        private void Timer_OnTimerTicked(float timerProgress)
        {
            _displayImage.fillAmount = 1 - timerProgress;
        }
    }
}
