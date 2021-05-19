using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{
    [System.Serializable]
    public class QuestStartNode : QuestNode
    {
        [SerializeField] string[] requieredStates;

        public override void execute()
        {
            Debug.Log("Execute QuestStartNode");
        }
    }
}
