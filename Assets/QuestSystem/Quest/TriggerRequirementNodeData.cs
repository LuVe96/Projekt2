using QuestSystem.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    [Serializable]
    public class TriggerRequirementNodeData : RequirementNodeData, IUnsubscribeEvent
    {

        [SerializeField] QuestTrigger questTrigger;

        bool addedAsObserver = false;

        public TriggerRequirementNodeData(string id)
        {
            this.uID = id;
        }

        public QuestTrigger QuestTrigger { get => questTrigger; set => questTrigger = value; }

        public override bool CheckRequirementNode()
        {
            if (!addedAsObserver)
            {
                QuestTrigger.OnTriggerChanged += TriggerHasChanged;
                addedAsObserver = true;
            }
            return QuestTrigger.CheckTriggerState();
        }

        public void UnsubscribeEvent()
        {
            QuestTrigger.OnTriggerChanged -= TriggerHasChanged;
        }

        private void TriggerHasChanged()
        {
            if (QuestTrigger.CheckTriggerState())
            {
                OnRequirementCheckPassed();
            }
        }

    }
}
