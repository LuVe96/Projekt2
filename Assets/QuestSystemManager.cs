using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestSystemManager : MonoBehaviour
    {

        private Quest.Quest[] quests;
        private List<Quest.Quest> availableQuests = new List<Quest.Quest>();


        // Start is called before the first frame update
        void Start()
        {
            quests = FindObjectsOfType<Quest.Quest>();

            foreach (Quest.Quest quest in quests)
            {
                // TODO: Chekc if available
                availableQuests.Add(quest);
                quest.StartQuest();
            }


        }

        // Update is called once per frame
        void Update()
        {

        }
    } 
}
