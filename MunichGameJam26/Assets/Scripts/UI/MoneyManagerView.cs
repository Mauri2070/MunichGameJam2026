using MGJ.Core;
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
            // TODO: formatting, € sign
            _moneyTextComponent.text = currentMoney.ToString();
        }
    }
}
