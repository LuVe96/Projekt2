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
        [SerializeField] string questName;
        [SerializeField] QuestState questState = QuestState.Inactive;
        //[SerializeField] List<QuestNodeData> nodeDatas = new List<QuestNodeData>();
        [SerializeField] List<QuestStartNodeData> startNodeDatas = new List<QuestStartNodeData>();
        [SerializeField] List<QuestDialogueNodeData> dialogNodeDatas = new List<QuestDialogueNodeData>();
        [SerializeField] List<StandartNodeData> standartNodeDatas = new List<StandartNodeData>();
        [SerializeField] List<EnableActionData> enableActionDatas = new List<EnableActionData>();
        [SerializeField] List<InventoryActionData> inventoryActionDatas = new List<InventoryActionData>();
        [SerializeField] List<InventoryRequireData> inventoryReqireNodeDatas = new List<InventoryRequireData>();
        [SerializeField] List<QuestEndNodeData> endNodeDatas = new List<QuestEndNodeData>();
        [SerializeField] List<VariableActionData> variableActionDatas = new List<VariableActionData>();
        [SerializeField] List<VariableRequireData> varaibleRequireDatas = new List<VariableRequireData>();
        [SerializeField] List<NoteNodeData> noteNodeDatas = new List<NoteNodeData>();


        Dictionary<string, QuestNodeData> nodeDataLookUp = new Dictionary<string, QuestNodeData>();

        List<QuestNodeData> aktiveNodeDatas = new List<QuestNodeData>();

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
                allNodes = AddToAllNodes(endNodeDatas.ToArray(), allNodes);
                allNodes = AddToAllNodes(variableActionDatas.ToArray(), allNodes);
                allNodes = AddToAllNodes(varaibleRequireDatas.ToArray(), allNodes);
                allNodes = AddToAllNodes(noteNodeDatas.ToArray(), allNodes);

                return allNodes;
            }
        }

        public string QuestName { get => questName; set => questName = value; }
        public QuestState QuestState { get => questState; set => questState = value; }
        public List<VariableActionData> VariableActionDatas { get => variableActionDatas; set => variableActionDatas = value; }
        public List<VariableRequireData> VaraibleRequireDatas { get => varaibleRequireDatas; set => varaibleRequireDatas = value; }

        private Quest()
        {
            if(Nodes.Count <= 0)
            {
                QuestStartNodeData startData = new QuestStartNodeData(Guid.NewGuid().ToString());
                startData.Rect = new Rect(50, 50, 200, 100);
                startNodeDatas.Add(startData);
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

        public void StartQuest()
        {
            OnValidate();
            if (Nodes.Count > 0)
            {
                aktiveNodeDatas.Add(Nodes[0]);
                (Nodes[0] as MainNodeData).execute(ContinueNodes, GetNodeByID);
            }

            QuestState = QuestState.Active;
        }

        void ContinueNodes(MainNodeData parentNode, DialogueEndPointContainer endPoint = null)
        {
            try
            {
                aktiveNodeDatas.Remove(parentNode);
                foreach (MainNodeData nodeData in GetChildsOfActive(parentNode, endPoint))
                {
                    aktiveNodeDatas.Add(nodeData);
                    if( nodeData is QuestEndNodeData)
                    {
                        (nodeData as QuestEndNodeData).setQuestEndDelegate(EndQuest); 
                    }
                    nodeData.execute(ContinueNodes, GetNodeByID);
                } 

            }
            catch (Exception)
            {
                Debug.Log("No Cilds found");
            }

        }

        private List<QuestNodeData> GetChildsOfActive(MainNodeData parentNode, DialogueEndPointContainer endPoint = null)
        {
            List<QuestNodeData> n = new List<QuestNodeData>();

            // getting childNodes
            foreach (string id in parentNode.ChildrenIDs)
            {
                if (nodeDataLookUp.ContainsKey(id))
                {
                    n.Add(nodeDataLookUp[id]);
                }
            }

            // getting endpointNodes
            if (endPoint != null)
            {
                foreach (string id in endPoint.endPointChilds)
                {
                    if (nodeDataLookUp.ContainsKey(id))
                    {
                        n.Add(nodeDataLookUp[id]);
                    }
                }
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

        void EndQuest(QuestEndType endType)
        {
            switch (endType)
            {
                case QuestEndType.Passed:
                    QuestState = QuestState.Passed;
                    break;
                case QuestEndType.Failed:
                    QuestState = QuestState.Failed;
                    break;
                default:
                    break;
            }

            // Destroy Nodes
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i] = null;
            }
            QuestSystemManager.Instance.UpdateQuestState();
        }

        //FOR_NEW: 05 Create new Data Instance
        public QuestNodeData CreateNewNode(QuestNodeType type)
        {

            Undo.RecordObject(this, "Create new Node");

            switch (type)
            {
                case QuestNodeType.StartNode:
                    QuestStartNodeData startData = new QuestStartNodeData(Guid.NewGuid().ToString());
                    startNodeDatas.Add(startData);
                    return startData;
                case QuestNodeType.DialogueNode:
                    QuestDialogueNodeData dialogueData = new QuestDialogueNodeData(Guid.NewGuid().ToString());
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
                    StandartNodeData stdData = new StandartNodeData(Guid.NewGuid().ToString());
                    standartNodeDatas.Add(stdData);
                    return stdData;
                case QuestNodeType.EndNode:
                    QuestEndNodeData endData = new QuestEndNodeData(Guid.NewGuid().ToString());
                    endNodeDatas.Add(endData);
                    return endData;
                case QuestNodeType.VariableRequirementNode:
                    VariableRequireData varRegData = new VariableRequireData(Guid.NewGuid().ToString());
                    varaibleRequireDatas.Add(varRegData);
                    return varRegData;
                case QuestNodeType.VariableActionNode:
                    VariableActionData varActData = new VariableActionData(Guid.NewGuid().ToString());
                    variableActionDatas.Add(varActData);
                    return varActData;
                case QuestNodeType.NoteNode:
                    NoteNodeData noteNodeData = new NoteNodeData(Guid.NewGuid().ToString());
                    noteNodeDatas.Add(noteNodeData);
                    return noteNodeData;
                default:
                    return null;
            }
        }

        //FOR_NEW: 06 Delete Data in List
        public void DeleteNode(QuestNodeData node)
        {
            DeleteAsChild(node);

            switch (node)
            {
                case QuestStartNodeData n:
                    startNodeDatas.Remove(n);
                    break;
                case QuestDialogueNodeData n:
                    dialogNodeDatas.Remove(n);
                    break;
                case InventoryRequireData n:
                    inventoryReqireNodeDatas.Remove(n);
                    break;
                case InventoryActionData n:
                    inventoryActionDatas.Remove(n);
                    break;
                case EnableActionData n:
                    enableActionDatas.Remove(n);
                    break; 
                case StandartNodeData n:
                    standartNodeDatas.Remove(n);
                    break;
                case QuestEndNodeData n:
                    endNodeDatas.Remove(n);
                    break;
                case VariableActionData n:
                    variableActionDatas.Remove(n);
                    break;
                case VariableRequireData n:
                    varaibleRequireDatas.Remove(n);
                    break;
                case NoteNodeData n:
                    noteNodeDatas.Remove(n);
                    break;
                default:
                    return;
            }
        }

        private void DeleteAsChild(QuestNodeData node)
        {
            switch (node)
            {
                case MainNodeData n:
                    foreach (QuestNodeData nodeData in Nodes)
                    {
                        if (!(nodeData is MainNodeData)) continue;
                        MainNodeData nData = (nodeData as MainNodeData);
                        if (nData.ChildrenIDs.Contains(node.UID))
                        {
                            nData.ChildrenIDs.Remove(node.UID);
                        }

                        if(nodeData is QuestDialogueNodeData)
                        {
                            foreach (DialogueEndPointContainer eP in (nodeData as QuestDialogueNodeData).DialogueEndPointContainer)
                            {
                                if (eP.endPointChilds.Contains(node.UID))
                                {
                                    (nodeData as QuestDialogueNodeData).RemoveDialogueEndPoint(eP.id, node.UID);
                                }                            
                            }                          
                        }
                    }
                    break;
                case RequirementNodeData n:
                    foreach (QuestNodeData nodeData in Nodes)
                    {
                        if (!(nodeData is MainNodeData)) continue;
                        MainNodeData nData = (nodeData as MainNodeData);
                        if (nData.RequirementIDs.Contains(node.UID))
                        {
                            nData.RequirementIDs.Remove(node.UID);
                        }
                    }
                    break;
                case ActionNodeData n:
                    foreach (QuestNodeData nodeData in Nodes)
                    {
                        if (!(nodeData is MainNodeData)) continue;
                        MainNodeData nData = (nodeData as MainNodeData);
                        if (nData.ActionIDs.Contains(node.UID))
                        {
                            nData.ActionIDs.Remove(node.UID);
                        }
                    }
                    break;

                default:
                    break;
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

    public enum QuestState
    {
        Inactive, Active, Failed, Passed
    }
}