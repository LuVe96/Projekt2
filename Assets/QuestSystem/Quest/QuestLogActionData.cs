using System.Collections;
using UnityEngine;

namespace QuestSystem.Quest
{
    [System.Serializable]
    public class QuestLogActionData : ActionNodeData
    {
        [SerializeField] string questLogName;
        [SerializeField] string questLogText;
        [SerializeField] bool toOtherQuest = false;

        [SerializeField] private QuestName ownQuestName;

        public QuestLogActionData(string id, QuestName questName)
        {
            this.uID = id;
            ownQuestName = questName;
        }

        public string QuestLogName { get => questLogName; set => questLogName = value; }
        public string QuestLogText { get => questLogText; set => questLogText = value; }
        public bool ToOtherQuest { get => toOtherQuest; set => toOtherQuest = value; }

        public override void executeAction()
        {
            Debug.Log("Execute Log Action");

            if (ToOtherQuest)
            {
                QuestLogManager.Instance.AddQuestLog(QuestLogName, QuestLogText);
            } else
            {
                QuestLogManager.Instance.AddQuestLog(ownQuestName.LogName, QuestLogText);
            }

        }
    }
}