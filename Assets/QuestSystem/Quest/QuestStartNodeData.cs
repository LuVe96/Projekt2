using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{
    [System.Serializable]
    public class QuestStartNodeData : MainNodeData
    {
        [SerializeField] public string[] requieredStates;

        [SerializeField] public string testStartString = "";

        public QuestStartNodeData(string id) : base(id)
        {
        }

        protected override void executeNode()
        {
            Debug.Log("Execute QuestStartNode");
            FinishNode();
        }
    }


}
 