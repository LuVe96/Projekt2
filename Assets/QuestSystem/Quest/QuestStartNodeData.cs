using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{
    [System.Serializable]
    public class QuestStartNodeData : MainNodeData
    {
        [SerializeField] string activeAfter = "";

        public QuestStartNodeData(string id) : base(id)
        {
        }

        public string ActiveAfter { get => activeAfter; set => activeAfter = value; }

        protected override void executeNode()
        {
            Debug.Log("Execute QuestStartNode");
            FinishNode();
        }

        protected override void resetNode()
        {
        }
    }


}
 