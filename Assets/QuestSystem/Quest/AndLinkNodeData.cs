using System.Collections;
using UnityEngine;

namespace QuestSystem.Quest
{
    [System.Serializable]
    public class AndLinkNodeData : MainNodeData
    {

        [SerializeField] int requiredExecutes = 1;

        private int currentExecutes = 0;

        public AndLinkNodeData(string id) : base(id)
        {
        }

        public int RequiredExecutes { get => requiredExecutes; set => requiredExecutes = value; }

        protected override void execute()
        {
            if(currentExecutes <= 0)
            {
                isActive = true;
            }

            if (!isActive) return;

            currentExecutes += 1;
            Debug.Log("And Link: curr: " + currentExecutes + " of: " + requiredExecutes);

            if (currentExecutes >= RequiredExecutes)
            {
                FinishNode();
            }
        }

        protected override void executeNode()
        {
        }

    }
}