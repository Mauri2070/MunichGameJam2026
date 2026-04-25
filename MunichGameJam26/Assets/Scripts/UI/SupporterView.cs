using MGJ.Supporter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MGJ.UI
{
    public class SupporterView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _supporterPercentageTextComponent;
        [SerializeField] private Image _supporterDiagraphImage;

        private void OnEnable()
        {
            SupporterManager.Instance.OnSupportChanged += SupporterManager_OnSupportChanged;
        }

        private void OnDisable()
        {
            SupporterManager.Instance.OnSupportChanged -= SupporterManager_OnSupportChanged;
        }

        private void Start()
        {
            UpdateView();
        }

        private void SupporterManager_OnSupportChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _supporterDiagraphImage.fillAmount = SupporterManager.Instance.SupporterPercentage;
            _supporterPercentageTextComponent.text = $"{Mathf.RoundToInt(SupporterManager.Instance.CurrentSupport)} %";
        }
    }
}
