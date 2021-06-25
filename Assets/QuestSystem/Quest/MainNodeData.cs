using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem.Quest
{

    public delegate void NodeHasFinished(MainNodeData parentNode, EndPointContainer endPoint = null);
    public delegate QuestNodeData GetNodeByID(string id);

    [System.Serializable]
    public abstract class MainNodeData : QuestNodeData
    {

        [SerializeField] List<string> childrenIDs = new List<string>();
        [SerializeField] List<string> requirementIDs = new List<string>();
        [SerializeField] List<string> actionIDs = new List<string>();
        [SerializeField] string title = null;

        public List<string> ChildrenIDs { get => childrenIDs; set => childrenIDs = value; }
        public List<string> RequirementIDs { get => requirementIDs; set => requirementIDs = value; }
        public List<string> ActionIDs { get => actionIDs; set => actionIDs = value; }
        public string Title { get => title; set => title = value; }

        bool isActive = false;

        [SerializeField]  protected NodeHasFinished NodeHasFinished;
        [SerializeField] protected GetNodeByID GetNodeByID;

        public MainNodeData(string uID)
        {
            this.uID = uID;
        }

        public void execute(NodeHasFinished nodeHasFinished, GetNodeByID getNodeByID)
        {
            NodeHasFinished = nodeHasFinished;
            GetNodeByID = getNodeByID;
            execute();
        }

        protected virtual void execute()
        {
            isActive = true;
            if (CheckRequirements())
            {
                executeNode();
            } else
            {
                resetNode();
            }

        }

        private void executeByRequirement()
        {
            if (isActive)
            {
                execute();
            }
        }

        protected bool CheckRequirements()
        {
            foreach (string reqId in requirementIDs)
            {
                try
                {
                   if( !(GetNodeByID(reqId) as RequirementNodeData).CheckRequirement(executeByRequirement))
                    {
                        return false;
                    }
                }
                catch (System.Exception)
                {
                    Debug.Log("Requirement Check Fialed");
                    throw;
                }
            }
            return true;
        }

        protected abstract void executeNode();
        protected virtual void resetNode(){}

        protected void FinishNode(EndPointContainer endPoint = null)
        {
            foreach (string actionID in ActionIDs)
            {
                try
                {
                    (GetNodeByID(actionID) as ActionNodeData).executeAction();
                }
                catch (System.Exception)
                {
                    Debug.Log("Action Execution Failed");
                    throw;
                }
            }

            //TODO: unsubscribe Requirements über Interface?
            foreach (string reqId in requirementIDs)
            {
                RequirementNodeData rnd = (GetNodeByID(reqId) as RequirementNodeData);
                if(rnd is IUnsubscribeEvent)
                {
                    (rnd as IUnsubscribeEvent).UnsubscribeEvent();
                }
            }
            isActive = false;
            NodeHasFinished(this, endPoint);
        }
    } 
}
