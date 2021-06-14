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
        //[SerializeField] List<QuestNodeData> nodeDatas = new List<QuestNodeData>();
        [SerializeField] List<QuestStartNodeData> startNodeDatas = new List<QuestStartNodeData>();
        [SerializeField] List<QuestDialogueNodeData> dialogNodeDatas = new List<QuestDialogueNodeData>();
        [SerializeField] List<StandartNodeData> standartNodeDatas = new List<StandartNodeData>();
        [SerializeField] List<EnableActionData> enableActionDatas = new List<EnableActionData>();
        [SerializeField] List<InventoryActionData> inventoryActionDatas = new List<InventoryActionData>();
        [SerializeField] List<InventoryRequireData> inventoryReqireNodeDatas = new List<InventoryRequireData>();


        Dictionary<string, QuestNodeData> nodeDataLookUp = new Dictionary<string, QuestNodeData>();

        QuestNodeData aktiveNodeData;

        //FOR_NEW: 04 Make List and add to Node Prop
        public List<QuestNodeData> Nodes { get
            {
                List<QuestNodeData> allNodes = new List<QuestNodeData>();
                allNodes = AddToAllNodes(startNodeDatas.ToArray(), allNodes);
                allNodes = AddToAllNodes(dialogNodeDatas.ToArray(), allNodes);
                allNodes = AddToAllNodes(enableActionDatas.ToArray(), allNodes);
                allNodes = AddToAllNodes(inventoryActionDatas.ToArray(), allNodes);
                allNodes = AddToAllNodes(inventoryReqireNodeDatas.ToArray(), allNodes);
                allNodes = AddToAllNodes(standartNodeDatas.ToArray(), allNodes);

                return allNodes;
            }
        }

        private List<QuestNodeData> AddToAllNodes(QuestNodeData[] nodes, List<QuestNodeData> allNodes )
        {
            foreach (var node in nodes)
            {
                allNodes.Add(node);
            }
            return allNodes;
        }

        //public Dialogue.Dialogue dia1;
        //public NPCDialogueAttacher nPCDialogueAttacher1;
        //public GameObject axt;

        //public LootItem lootItem;

        private void Start()
        {
            //QuestStartNodeData squestdata = new QuestStartNodeData("Start_1", ContinueNodes, GetNodeByID);
            //squestdata.ChildrenIDs.Add("Dialog_1");
            //startNodeDatas.Add(squestdata);

            //QuestDialogueNodeData qqdata = new QuestDialogueNodeData("Dialog_1", ContinueNodes, GetNodeByID);
            //qqdata.Dialogue = dia1;
            //qqdata.NPCDialogueAttacher = nPCDialogueAttacher1;
            //qqdata.ActionIDs.Add("Enable_1");
            //qqdata.ChildrenIDs.Add("Dialog_2");
            //dialogNodeDatas.Add(qqdata);

            //EnableActionData enData = new EnableActionData("Enable_1", axt);
            //enableActionDatas.Add(enData);

            //QuestDialogueNodeData qqdata2 = new QuestDialogueNodeData("Dialog_2", ContinueNodes, GetNodeByID);
            //qqdata2.Dialogue = dia1;
            //qqdata2.NPCDialogueAttacher = nPCDialogueAttacher1;
            //qqdata2.RequirementIDs.Add( "InventReq_1");
            //qqdata2.ActionIDs.Add("Inventory_1");
            //dialogNodeDatas.Add(qqdata2);

            //InventoryActionData inData = new InventoryActionData("Inventory_1", InventorySelectionType.Remove, lootItem, 1);
            //inventoryActionDatas.Add(inData);

            //InventoryRequireData inReData = new InventoryRequireData("InventReq_1", lootItem, 1);
            //inventoryReqireNodeDatas.Add(inReData);
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
            try
            {
                aktiveNodeData = GetChildOfActive(nextChildIndex);
            }
            catch (Exception)
            {
                Debug.Log("No Cilds found");
            }
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

        //FOR_NEW: 05 Create new Data Instance
        public QuestNodeData CreateNewNode(QuestNodeType type)
        {

            Undo.RecordObject(this, "Create new Node");

            switch (type)
            {
                case QuestNodeType.StartNode:
                    QuestStartNodeData startData = new QuestStartNodeData(Guid.NewGuid().ToString(), ContinueNodes, GetNodeByID);
                    startNodeDatas.Add(startData);
                    return startData;
                case QuestNodeType.DialogueNode:
                    QuestDialogueNodeData dialogueData = new QuestDialogueNodeData(Guid.NewGuid().ToString(), ContinueNodes, GetNodeByID);
                    dialogNodeDatas.Add(dialogueData);
                    return dialogueData;
                case QuestNodeType.InventoryRequirementNode:
                    InventoryRequireData invReqData = new InventoryRequireData(Guid.NewGuid().ToString());
                    inventoryReqireNodeDatas.Add(invReqData);
                    return invReqData;
                case QuestNodeType.InventoryActionNode:
                    InventoryActionData invActData = new InventoryActionData(Guid.NewGuid().ToString());
                    inventoryActionDatas.Add(invActData);
                    return invActData;
                case QuestNodeType.EnableActionNode:
                    EnableActionData enActData = new EnableActionData(Guid.NewGuid().ToString());
                    enableActionDatas.Add(enActData);
                    return enActData;
                case QuestNodeType.StandartNode:
                    StandartNodeData stdData = new StandartNodeData(Guid.NewGuid().ToString(), ContinueNodes, GetNodeByID);
                    standartNodeDatas.Add(stdData);
                    return stdData;
                default:
                    return null;
            }


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