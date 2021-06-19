using QuestSystem.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestSystemManager : MonoBehaviour
    {
        public static QuestSystemManager Instance = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);  // the Singelton Obj gets not deleted when change szene
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private Quest.Quest[] quests;
        Dictionary<string, QuestState> questsLookUp = new Dictionary<string, QuestState>();



        // Start is called before the first frame update
        void Start()
        {
            quests = FindObjectsOfType<Quest.Quest>();

            foreach (Quest.Quest quest in quests)
            {
                questsLookUp.Add(quest.QuestName, quest.QuestState);
                if ((quest.Nodes[0] as QuestStartNodeData).ActiveAfter == "None")
                    quest.StartQuest();
            }


        }

        // Update is called once per frame
        void UpdateQuestLookUp()
        {
            questsLookUp.Clear();
            foreach (Quest.Quest quest in quests)
            {
                questsLookUp.Add(quest.QuestName, quest.QuestState);
            
            }
        }

        internal void UpdateQuestState()
        {
            UpdateQuestLookUp();
            foreach (Quest.Quest quest in quests)
            {
                string activeReq = (quest.Nodes[0] as QuestStartNodeData).ActiveAfter;
                if (quest.QuestState == QuestState.Inactive && questsLookUp.ContainsKey(activeReq) )
                {
                    if( questsLookUp[activeReq] == QuestState.Failed || questsLookUp[activeReq] == QuestState.Passed)
                    {
                        quest.StartQuest();
                    }
                }
            }
        }
    } 
}
