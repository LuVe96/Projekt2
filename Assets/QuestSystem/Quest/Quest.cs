using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QuestSystem.Quest
{
    //[CreateAssetMenu(fileName = "New Quest", menuName = "QuestSystem/Quest", order = 0)]
    public class Quest : MonoBehaviour
    {
        public string questName;
        [SerializeField] List<QuestNodeData> nodeDatas = new List<QuestNodeData>();
        [SerializeField] List<QuestStartNodeData> startNodeDatas = new List<QuestStartNodeData>();
        [SerializeField] List<QuestDialogueNodeData> dialogNodeDatas = new List<QuestDialogueNodeData>();
        [SerializeField] List<RequirementNodeData> reqireNodeDatas = new List<RequirementNodeData>();
        [SerializeField] List<EnableActionData> enableActionDatas = new List<EnableActionData>();
        Dictionary<string, QuestNodeData> nodeDataLookUp = new Dictionary<string, QuestNodeData>();

        QuestNodeData aktiveNodeData;

        //FOR_NEW: Make List and add to Node Prop
        public List<QuestNodeData> Nodes { get
            {
                List<QuestNodeData> allNodes = new List<QuestNodeData>();
                foreach (var node in startNodeDatas)
                {
                    allNodes.Add(node);
                }
                foreach (var node in reqireNodeDatas)
                {
                    allNodes.Add(node);
                }
                foreach (var node in dialogNodeDatas)
                {
                    allNodes.Add(node);
                }
                foreach (var node in enableActionDatas)
                {
                    allNodes.Add(node);
                }

                //return allNodes;
                return nodeDatas;
            }
        }

        public Dialogue.Dialogue dia1;
        public NPCDialogueAttacher nPCDialogueAttacher1;
        public GameObject axt;

        private void Start()
        {
            QuestStartNodeData squestdata = new QuestStartNodeData("Start_1", ContinueNodes, GetNodeByID);
            squestdata.ChildrenIDs.Add("Dialog_1");
            nodeDatas.Add(squestdata);

            QuestDialogueNodeData qqdata = new QuestDialogueNodeData("Dialog_1", ContinueNodes, GetNodeByID);
            qqdata.Dialogue = dia1;
            qqdata.NPCDialogueAttacher = nPCDialogueAttacher1;
            qqdata.ActionIDs.Add("Enable_1");
            nodeDatas.Add(qqdata);

            EnableActionData enData = new EnableActionData("Enable_1", axt);
            nodeDatas.Add(enData);
        }

        public void StartQuest()
        {
            OnValidate();
            if (Nodes.Count > 0)
            {
                aktiveNodeData = Nodes[0];
                (aktiveNodeData as MainNodeData).execute();

            }
        }

        void ContinueNodes(int nextChildIndex)
        {
            aktiveNodeData = GetChildOfActive(nextChildIndex);
            (aktiveNodeData as MainNodeData).execute();
        }

        private QuestNodeData GetChildOfActive(int index)
        {
            QuestNodeData n = null;
            string id = (aktiveNodeData as MainNodeData).ChildrenIDs[index];
            if (nodeDataLookUp.ContainsKey(id))
            {
                n = nodeDataLookUp[id];
            }

            return n;
        }

        private QuestNodeData GetNodeByID(string id)
        {
            QuestNodeData n = null;
            if (nodeDataLookUp.ContainsKey(id))
            {
                n = nodeDataLookUp[id];
            }

            return n;
        }

        public IEnumerable<QuestNodeData> getAllNodes()
        {
            return Nodes;
        }

        public QuestNodeData CreateNewNode(QuestNodeType type)
        {

            Undo.RecordObject(this, "Create new Node");

            //switch (type)
            //{
            //    case QuestNodeType.StartNode:
            //        QuestStartNodeData squestdata = new QuestStartNodeData();
            //        squestdata.UID = Guid.NewGuid().ToString();
            //        startNodeDatas.Add(squestdata);
            //        OnValidate();
            //        return squestdata;
            //    case QuestNodeType.RequirementNode:
            //        RequirementNodeData rquestdata = new RequirementNodeData();
            //        rquestdata.UID = Guid.NewGuid().ToString();
            //        reqireNodeDatas.Add(rquestdata);
            //        OnValidate();
            //        return rquestdata;
            //    case QuestNodeType.DialogueNode:
            //        return null;
            //        break;
            //    default:
            return null;
            //        break;
            //}


        }

      

        private void OnValidate()
        {
            nodeDataLookUp.Clear();
            foreach (QuestNodeData _node in Nodes)
            {
                nodeDataLookUp[_node.UID] = _node;
            }

        }

    }

}