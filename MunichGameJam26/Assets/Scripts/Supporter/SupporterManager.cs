using System;
using UnityEngine;

namespace MGJ.Supporter
{
    public class SupporterManager : MonoBehaviour
    {
        private static SupporterManager _instance;
        public static SupporterManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<SupporterManager>();
                    if (_instance == null)
                    {
                        Debug.LogWarning("Could not find Supporter Mangager. Creating one.");
                        GameObject go = new GameObject("Supporter Manager (created).");
                        _instance = go.AddComponent<SupporterManager>();
                    }
                }
                return _instance;
            }
        }

        public event Action OnSupportChanged;

        private float _currentSupport;
        public float CurrentSupport => _currentSupport;

        public float SupporterPercentage => Mathf.Clamp01(_currentSupport / 100);

        private void Awake()
        {
            _currentSupport = 0;
        }

        public void AddSupporter(float supporter)
        {
            _currentSupport += supporter;
            OnSupportChanged?.Invoke();
        }
    }
}
