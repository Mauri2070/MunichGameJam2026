using UnityEngine;

namespace MGJ.Idle
{
    [CreateAssetMenu(fileName = "IdleActivityDefinition", menuName = "MGJ26/IdleActivityDefinition")]
    public class IdleActivityDefinition : ScriptableObject
    {
        [Header("Activity Settings")]
        public string ActivityName;
        public float BaseRessourceGeneration;
        public float BaseActivityTime;
        public float FinalActivityTime;
        public IdleActivityMilestone[] Milestones;

        [Header("Activity Visuals")]
        public Sprite ActivityIcon;

        public bool HasUpgradesLeft(int currentMilestone, int stepInMilestone)
        {
            if (currentMilestone < 0 || currentMilestone >= Milestones.Length)
            {
                return false;
            }

            return stepInMilestone < Milestones[currentMilestone].UpgradeSteps;
        }

        public bool IsMilestoneUpgrade(int currentMilestone, int stepInMilestone)
        {
            if (currentMilestone < 0 || currentMilestone >= Milestones.Length)
            {
                Debug.LogWarning("Trying to access invalid upgrade.");
                return false;
            }

            return Milestones[currentMilestone].UpgradeSteps - 1 == stepInMilestone;
        }

        public float GetCostForUpgrade(int milestone, int stepInMilestone)
        {
            if (milestone < 0 || milestone >= Milestones.Length)
            {
                Debug.LogWarning("Trying to access invalid upgrade.");
                return -1.0f;
            }

            if (stepInMilestone >= Milestones[milestone].UpgradeSteps)
            {
                Debug.LogWarning("Trying to access invalid upgrade.");
                return -1.0f;
            }

            float cost = CalculateUpgadeCost(Milestones[milestone].StartUpgradeCost, Milestones[milestone].EndUpgradeCost,
                Milestones[milestone].UpgradeSteps, stepInMilestone);
            //Debug.Log($"Upgrade cost for activity {ActivityName} milestone {milestone}, step {stepInMilestone} is {cost}");
            return cost;
        }

        private float CalculateUpgadeCost(float minCost, float maxCost, int totalSteps, int step)
        {
            if (totalSteps == 1)
            {
                return minCost;
            }

            // step in [0, totalSteps-1] => [0,1]
            // f(x) = (step/totalSteps-1)^2 * (max-min) + min
            return Mathf.Pow((float)step / (totalSteps - 1), 2) * (maxCost - minCost) + minCost;
        }
    }
}
