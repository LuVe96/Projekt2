using System.Collections;
using UnityEngine;

namespace QuestSystem.Quest
{
    [System.Serializable]
    public class VariableRequireData : RequirementNodeData
    {
        [SerializeField] string variableName;
        [SerializeField] string requiredVarialbeValue;
        private string v;

        public VariableRequireData(string id)
        {
            uID = id;
        }

        public string RequiredVarialbeValue { get => requiredVarialbeValue; set => requiredVarialbeValue = value; }
        public string VariableName { get => variableName; set => variableName = value; }

        public override bool CheckRequirementNode()
        {
            return QuestSystemManager.Instance.getQuestVariableValue(variableName) == requiredVarialbeValue;
        }
    }
}