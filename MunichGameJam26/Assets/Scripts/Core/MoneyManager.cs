using System;
using UnityEngine;

namespace MGJ.Core
{
    public class MoneyManager : MonoBehaviour
    {
        private static MoneyManager _instance;
        public static MoneyManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<MoneyManager>();
                    if (_instance == null)
                    {
                        Debug.LogWarning("Could not find MoneyManager. Creating one.");
                        GameObject go = new GameObject("Money Manager (created)");
                        _instance = go.AddComponent<MoneyManager>();
                    }
                }
                return _instance;
            }
        }

        public event Action<float> OnMoneyChanged;

        [SerializeField] private float _startingMoney = 1.0f;

        private float _money;
        public float Money => _money;

        private void Start()
        {
            _money = _startingMoney;
            OnMoneyChanged?.Invoke(_money);
        }

        public bool CanPay(float cost)
        {
            return _money >= cost;
        }

        public void Pay(float cost)
        {
            if (!CanPay(cost))
            {
                Debug.LogError("Cannot pay! Make sure to check CanPay first!");
                return;
            }

            _money -= cost;
            OnMoneyChanged?.Invoke(_money);
        }

        public void EarnMoney(float ammount)
        {
            _money += ammount;
            OnMoneyChanged?.Invoke(_money);
        }
    }
}
