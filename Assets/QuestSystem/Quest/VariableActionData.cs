using System.Collections;
using UnityEngine;

namespace QuestSystem.Quest
{
    [System.Serializable]
    public class VariableActionData : ActionNodeData
    {

        [SerializeField] string variableName;
        [SerializeField] string variableValue;

        public VariableActionData(string id)
        {
            uID = id;
        }

        public string VariableName { get => variableName; set => variableName = value; }
        public string VariableValue { get => variableValue; set => variableValue = value; }

        public override void executeAction()
        {
            QuestSystemManager.Instance.setQuestVariable(variableName, VariableValue);
        }
    }
}