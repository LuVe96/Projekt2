using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem.Quest
{
    [System.Serializable]
    public class EnableActionData : ActionNode
    {
        GameObject gObject;
        bool disable = false;

        public EnableActionData(string id, GameObject gObject, bool disable = false)
        {
            this.gObject = gObject;
            this.disable = disable;
            this.uID = id;
        }

        public override void executeAction()
        {
            Debug.Log("Execute Enable Action");

            if(gObject != null)
            {
                gObject.SetActive(!disable);
            }
            else
            {
                Debug.LogWarning("No GameObject set on Enable Action");
            }
        }
    }

}