using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem.Quest
{

    public delegate void NodeHasFinished(int nextChildIndex);
    public delegate QuestNodeData GetNodeByID(string id);

    [System.Serializable]
    public abstract class MainNodeData : QuestNodeData
    {

        [SerializeField] List<string> childrenIDs = new List<string>();
        [SerializeField] List<string> requirementIDs = new List<string>();
        [SerializeField] List<string> actionIDs = new List<string>();

        public List<string> ChildrenIDs { get => childrenIDs; set => childrenIDs = value; }
        public List<string> RequirementIDs { get => requirementIDs; set => requirementIDs = value; }
        public List<string> ActionIDs { get => actionIDs; set => actionIDs = value; }

        bool isActive = false;

        protected NodeHasFinished NodeHasFinished;
        protected GetNodeByID GetNodeByID;

        public MainNodeData(string uID, NodeHasFinished nodeHasFinished, GetNodeByID getNodeByID)
        {
            this.uID = uID;
            NodeHasFinished = nodeHasFinished;
            GetNodeByID = getNodeByID;
        }

        public void execute()
        {
            isActive = true;
            if (CheckRequirements())
            {
                executeNode();
            } else
            {
                // UNEXECUTE
            }

        }

        private void executeByRequirement()
        {
            if (isActive)
            {
                execute();
            }
        }

        private bool CheckRequirements()
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

                    throw;
                }
            }
            return true;
        }

        protected abstract void executeNode();


        protected void FinishNode(int nextChildIndex)
        {
            foreach (string actionID in ActionIDs)
            {
                try
                {
                    (GetNodeByID(actionID) as ActionNodeData).executeAction();
                }
                catch (System.Exception)
                {

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
            NodeHasFinished(nextChildIndex);
        }
    } 
}
