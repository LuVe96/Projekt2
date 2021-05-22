using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{
    //[CreateAssetMenu(fileName = "New QuestDialogueNode", menuName = "QuestSystem/QuestDialogueNode", order = 0)]
    public class QuestDialogueNodeData : QuestNodeData
    {
        [SerializeField] QuestSystem.Dialogue.Dialogue dialogue;
        [SerializeField] NPCDialogueAttacher nPCDialogueAttacher;
        [SerializeField] Test test;


        public override void execute()
        {
            Debug.Log("Execute QuestDialogueNode");
            //nPCDialogueAttacher.AddDialogue(dialogue);
        }

        private void OnEnable()
        {
        }
    }
}

[System.Serializable]
public class Test
{
    public NPCDialogueAttacher nPCDialogueAttacher;
}
