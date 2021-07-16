using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace QuestSystem
{
    [CreateAssetMenu(fileName = "New QuestVariableObject", menuName = "QuestSystem/QuestVariableObject", order = 0)]
    public class QuestVariableObject : ScriptableObject
    {
        [SerializeField] List<QuestVariableTemplate> questVariableTemplates = new List<QuestVariableTemplate>();

        public void AddQuestVarialbe(QuestVariableTemplate variable)
        {
            questVariableTemplates.Add(variable);
        }

        //public void RemoveQuestName(string newName)
        //{
        //    questNames.Remove(newName);
        //}

        public List<QuestVariableTemplate> GetAllQuestVariableTemplates()
        {
            return questVariableTemplates;
        }

        internal string GetCalculatedValueForVariable(string title, string value, VariableSetterType setterType, int steps)
        {
            QuestVariableTemplate variable = questVariableTemplates.Find((v) => v.Title == title);


           int curIndex = variable.Datas.IndexOf(value != null ? value : variable.InitialValue);

            if (setterType == VariableSetterType.increaseSteps)
            {
                int i = curIndex - steps;
                return variable.Datas[i < 0 ? 0 : i];

            } else if  (setterType == VariableSetterType.decreaseSteps)
            {
                int i = curIndex + steps;
                return variable.Datas[i >= variable.Datas.Count ? (variable.Datas.Count -1) : i];
            }

            return null;
        }

        internal bool CheckVariableValue(string title, string requiredValue, VariableGetterType getterType, string currentValue)
        {
            QuestVariableTemplate variable = questVariableTemplates.Find((v) => v.Title == title);


            int curIndex = variable.Datas.IndexOf(currentValue != null ? currentValue : variable.InitialValue);
            int requiredIndex = variable.Datas.IndexOf(requiredValue);

            if (getterType == VariableGetterType.heigherThan)
            { 
                return curIndex < requiredIndex;

            }
            else if (getterType == VariableGetterType.lowerThan)
            {
                return curIndex > requiredIndex;
            }

            return false;
        }

        internal string GetInitalValueOf(string title)
        {
            return questVariableTemplates.Find((v) => v.Title == title).InitialValue;
        }
    }

    [System.Serializable]
    public class QuestVariableTemplate
    {
        [SerializeField] string title = "";
        [SerializeField] QuestVariableType type = QuestVariableType.Bool;
        [SerializeField] List<string> datas = new List<string>();
        [SerializeField] string initialValue = "";

        public QuestVariableTemplate()
        { 
        }

        public string Title { get => title; set => title = value; }
        public List<string> Datas { get => datas; set => datas = value; }
        public QuestVariableType Type { get => type; set
            {
                switch (value)
                {
                    case QuestVariableType.Bool:
                        this.Datas.Clear();
                        this.datas.Add("True");
                        this.datas.Add("False");
                        break;
                    case QuestVariableType.State:
                        break;
                    default:
                        break;
                }

                type = value;
            }
        }

        public string InitialValue { get => initialValue; set => initialValue = value; }

        //public QuestVariableTemplate(string title, QuestVariableType type, List<string> datas = null)
        //{
        //    this.title = title;
        //    this.type = type;

        //    if(datas == null)
        //    {
        //        if( type == QuestVariableType.Bool)
        //        {
        //            this.datas.Add("True");
        //            this.datas.Add("False");
        //        }
        //    }
        //    else
        //    {
        //        this.datas = datas;
        //    }
        //}


    }

    public enum QuestVariableType
    {
        Bool, State
    }

}