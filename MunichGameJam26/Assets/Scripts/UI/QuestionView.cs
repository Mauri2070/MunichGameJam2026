using MGJ.Boosts;
using MGJ.Questions;
using TMPro;
using UnityEngine;

namespace MGJ.UI
{
    public class QuestionView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _questionDisplayParent;
        [SerializeField] private TextMeshProUGUI _questionTextComponent;
        [SerializeField] private TextMeshProUGUI _explanationTextComponent;

        [Header("Settings")]
        [SerializeField] private string _explanationWrongPrefix = "<align=center><b>Falsch!</b><align=left>\n\n";
        [SerializeField] private string _explanationRightPrefix = "<align=center><b>Richtig!</b><align=left>\n\n";

        private bool _explanationDisplayed;

        private BoostType _targetBoost;

        private void Awake()
        {
            _explanationDisplayed = false;
        }

        private void OnEnable()
        {
            QuestionsManager.Instance.OnCurrentQuestionChanged += QuestionsManager_OnCurrentQuestionChanged;
        }

        private void OnDisable()
        {
            QuestionsManager.Instance.OnCurrentQuestionChanged -= QuestionsManager_OnCurrentQuestionChanged;
        }

        private void QuestionsManager_OnCurrentQuestionChanged()
        {
            _questionTextComponent.text = QuestionsManager.Instance.CurrentQuestion.Question;
            _explanationTextComponent.text = string.Empty;
            _questionDisplayParent.SetActive(true);
            _explanationDisplayed = false;
        }

        public void StatementWrongSelected()
        {
            HandleButton();
        }

        public void StatementRightSelected()
        {
            HandleButton();
        }

        private void HandleButton()
        {
            if (_explanationDisplayed)
            {
                _questionDisplayParent.SetActive(false);
                BoostsManager.Instance.ApplyBoost(_targetBoost);
            }
            else
            {
                string prefix = QuestionsManager.Instance.CurrentQuestion.IsCorrect
                    ? _explanationRightPrefix :
                    _explanationWrongPrefix;

                _explanationTextComponent.text = $"{prefix}{QuestionsManager.Instance.CurrentQuestion.Explanation}";
                _explanationDisplayed = true;
            }
        }

        public void SetMultiplierBoost()
        {
            _targetBoost = BoostType.Earnings;
        }

        public void SetTimeWarpBoost()
        {
            _targetBoost = BoostType.TimeWarp;
        }
    }
}
