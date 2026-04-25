using UnityEngine;

namespace MGJ.Questions
{
    [CreateAssetMenu(fileName = "QuestionDef", menuName = "MGJ/QuestionDef")]
    public class QuestionDefinition : ScriptableObject
    {
        public string Question;
        public bool IsCorrect;
        public string Explanation;
    }
}
