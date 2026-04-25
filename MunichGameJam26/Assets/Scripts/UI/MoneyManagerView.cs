using MGJ.Core;
using MGJ.Utility;
using TMPro;
using UnityEngine;

namespace MGJ.UI
{
    public class MoneyManagerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyTextComponent;

        private void OnEnable()
        {
            MoneyManager.Instance.OnMoneyChanged += MoneyManager_OnMoneyChanged;
        }

        private void OnDisable()
        {
            MoneyManager.Instance.OnMoneyChanged -= MoneyManager_OnMoneyChanged;
        }

        private void MoneyManager_OnMoneyChanged(float currentMoney)
        {
            _moneyTextComponent.text = currentMoney.ToEuroString();
        }
    }
}
