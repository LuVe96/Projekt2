using System.Collections;
using UnityEngine;

namespace QuestSystem.Quest
{
    [System.Serializable]
    public class OtherQuestEndActionData : ActionNodeData
    {

        [SerializeField] Quest otherQuest;
        [SerializeField] QuestEndType questEndType = QuestEndType.Passed;

        public OtherQuestEndActionData(string id)
        {
            uID = id;
        }

        public Quest OtherQuest { get => otherQuest; set => otherQuest = value; }
        public QuestEndType QuestEndType { get => questEndType; set => questEndType = value; }

        public override void executeAction()
        {
            if(otherQuest!= null)
            {
                otherQuest.EndQuest(QuestEndType);
            }
        }
    }
}