using UnityEngine;
using UnityEngine.Events;

namespace MGJ.Supporter
{
    public class SupporterWinConditionHandler : MonoBehaviour
    {
        [SerializeField, Range(1f, 99f)] private float _supporterThreshold = 50f;

        public UnityEvent OnWinConditionMet;

        private void OnEnable()
        {
            SupporterManager.Instance.OnSupportChanged += SupporterManager_OnSupportChanged;
        }

        private void OnDisable()
        {
            SupporterManager.Instance.OnSupportChanged -= SupporterManager_OnSupportChanged;
        }

        private void SupporterManager_OnSupportChanged()
        {
            if (SupporterManager.Instance.CurrentSupport >= _supporterThreshold)
            {
                OnWinConditionMet?.Invoke();
            }
        }
    }
}
