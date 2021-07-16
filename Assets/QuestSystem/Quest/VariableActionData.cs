using System.Collections;
using UnityEngine;

namespace QuestSystem.Quest
{
    [System.Serializable]
    public class VariableActionData : ActionNodeData
    {

        [SerializeField] string variableName;
        [SerializeField] string variableValue;
        [SerializeField] VariableSetterType setterType = VariableSetterType.setTo;
        [SerializeField] int steps = 0;

        public VariableActionData(string id)
        {
            uID = id;
        }

        public string VariableName { get => variableName; set => variableName = value; }
        public string VariableValue { get => variableValue; set => variableValue = value; }
        public VariableSetterType SetterType { get => setterType; set => setterType = value; }
        public int Steps { get => steps; set => steps = value; }

        public override void executeAction()
        {
            QuestSystemManager.Instance.SetQuestVariable(variableName, VariableValue, setterType, steps);
        }
    }
}