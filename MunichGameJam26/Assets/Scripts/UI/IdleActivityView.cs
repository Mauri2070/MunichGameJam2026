using MGJ.Idle;
using MGJ.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MGJ.UI
{
    public class IdleActivityView : MonoBehaviour
    {
        [SerializeField] private IdleActivityController _targetActivityController;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _upgradeTextComponent;
        [SerializeField] private TextMeshProUGUI _activityNameTextComponent;
        [SerializeField] private TextMeshProUGUI _outputTextComponent;
        [SerializeField] private Image _activityIconImageComponent;

        [Header("Text")]
        [SerializeField] private string _upgradeCostPrefix = "Upgrade: ";
        [SerializeField] private string _maxedOutString = "Maximum erreicht";

        private void OnEnable()
        {
            _targetActivityController.OnActivityStateChanged += IdleActivityController_OnActivityStateChanged;

            _activityNameTextComponent.text = _targetActivityController.Activity.ActivityName;
            _activityIconImageComponent.sprite = _targetActivityController.Activity.ActivityIcon;
            UpdateView();
        }

        private void OnDisable()
        {
            _targetActivityController.OnActivityStateChanged -= IdleActivityController_OnActivityStateChanged;
        }

        private void Start()
        {
            UpdateView();
        }

        private void IdleActivityController_OnActivityStateChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            // TODO: formatting + €
            if (_targetActivityController.CanBeUpgraded())
            {
                _upgradeTextComponent.text = _upgradeCostPrefix + _targetActivityController.GetCostForNextUpgrade().ToEuroString();
            }
            else
            {
                _upgradeTextComponent.text = _maxedOutString;
            }

            _outputTextComponent.text = _targetActivityController.CurrentActivityOutput.ToEuroString();
        }
    }
}
