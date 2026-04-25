using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGJ.Questions
{
    public class QuestionsManager : MonoBehaviour
    {
        private static QuestionsManager _instance;
        public static QuestionsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<QuestionsManager>();
                    if (_instance == null)
                    {
                        Debug.LogWarning("Could not find QuestionsManager. Creating one (which will not have any questions).");
                        GameObject go = new("Questions Manager (Dummy)");
                        _instance = go.AddComponent<QuestionsManager>();
                    }
                }
                return _instance;
            }
        }

        public event Action OnCurrentQuestionChanged;

        [SerializeField] private List<QuestionDefinition> _questions;

        private List<QuestionDefinition> _availableQuestions;

        private QuestionDefinition _currentQuestion;
        public QuestionDefinition CurrentQuestion => _currentQuestion;

        private void Awake()
        {
            _availableQuestions = new List<QuestionDefinition>();
        }

        public void DisplayQuestion()
        {
            if (_availableQuestions.Count == 0)
            {
                _availableQuestions.AddRange(_questions);
            }

            int questionIdx = UnityEngine.Random.Range(0, _availableQuestions.Count);
            _currentQuestion = _availableQuestions[questionIdx];
            _availableQuestions.RemoveAt(questionIdx);

            OnCurrentQuestionChanged?.Invoke();
        }
    }
}
