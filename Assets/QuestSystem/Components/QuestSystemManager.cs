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

        private List<QuestVariable> questVariables = new List<QuestVariable>();
        Dictionary<string, QuestVariable> questVariablesLookUp = new Dictionary<string, QuestVariable>();

        QuestVariableObject questVariableResource;



        // Start is called before the first frame update
        void Start()
        {
            quests = FindObjectsOfType<Quest.Quest>();
            questVariableResource = Resources.Load("QuestVariables") as QuestVariableObject;

            questsLookUp.Clear();
            foreach (Quest.Quest quest in quests)
            {
                questsLookUp.Add(quest.QuestName.Name, quest.QuestState);
                if ((quest.Nodes[0] as QuestStartNodeData).ActiveAfter == "None")
                    quest.StartQuest();
            }
        }

        public void SetQuestVariable(string title, string value, VariableSetterType setterType, int steps)
        {

            if (questVariablesLookUp.ContainsKey(title))
            {
                if(setterType == VariableSetterType.setTo)
                {
                    questVariablesLookUp[title].value = value;
                } else
                {
                    questVariablesLookUp[title].value = questVariableResource.GetCalculatedValueForVariable(title, questVariablesLookUp[title].value, setterType, steps);
                }

            }else
            {
                if( setterType == VariableSetterType.setTo)
                {
                    questVariables.Add(new QuestVariable(title, value));
                } else
                {
                    questVariables.Add(new QuestVariable(title, questVariableResource.GetCalculatedValueForVariable(title, null, setterType, steps)));
                }
            }

            questVariablesLookUp.Clear();
            foreach (QuestVariable var in questVariables)
            {
                questVariablesLookUp.Add(var.title, var);

            }

        }

        public bool CheckQuestVariableValue(string title, string requiredValue, VariableGetterType getterType)
        {
            if (questVariablesLookUp.ContainsKey(title))
            {
                if ( getterType == VariableGetterType.equals)
                {
                    return questVariablesLookUp[title].value == requiredValue;
                }
                else
                {
                    return questVariableResource.CheckVariableValue(title, requiredValue, getterType, questVariablesLookUp[title].value);
                }

            }
            else
            {
                if (getterType == VariableGetterType.equals)
                {
                    return questVariableResource.GetInitalValueOf(title) == requiredValue;
                }
                else
                {
                    return questVariableResource.CheckVariableValue(title, requiredValue, getterType, null);
                }

            }  
        }

        // Update is called once per frame
        void UpdateQuestLookUp()
        {
            questsLookUp.Clear();
            foreach (Quest.Quest quest in quests)
            {
                questsLookUp.Add(quest.QuestName.Name, quest.QuestState);
            
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

    public class QuestVariable
    {
        public string title;
        public string value;

        public QuestVariable(string title, string value)
        {
            this.title = title;
            this.value = value;
        }
    }
}
