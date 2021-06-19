using System.Collections;
using UnityEngine;

namespace QuestSystem.Quest
{
    public delegate void QuestHasFinished(QuestEndType endType);

    [System.Serializable]
    public class QuestEndNodeData : MainNodeData
    {

        [SerializeField] QuestEndType endType = QuestEndType.Passed;
        QuestHasFinished QuestHasFinished;

        public QuestEndNodeData(string id) : base(id)
        {
        }

        public QuestEndType EndType { get => endType; set => endType = value; }

        public void setQuestEndDelegate(QuestHasFinished questHasFinished)
        {
            QuestHasFinished = questHasFinished;
        }

        protected override void executeNode()
            {
                FinishNode();
                QuestHasFinished(EndType);
            }
    }

    [System.Serializable]
    public enum QuestEndType
    {
        Passed, Failed
    }

}