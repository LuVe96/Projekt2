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
        [SerializeField] List<QuestNodeData> nodes = new List<QuestNodeData>();
        [SerializeField] List<QuestStartNodeData> startNodes = new List<QuestStartNodeData>();
        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);
        Dictionary<string, QuestNodeData> nodeLookUp = new Dictionary<string, QuestNodeData>();

        public List<QuestNodeData> Nodes { get
            {
                List<QuestNodeData> allNodes = new List<QuestNodeData>();
                foreach (var node in startNodes)
                {
                    allNodes.Add(node);
                }

                return allNodes;
            }
        }

        private void Start()
        {
            
        }

        public IEnumerable<QuestNodeData> getAllNodes()
        {
            return Nodes;
        }

        public QuestNodeData CreateNewNode(QuestNodeType type)
        {

            Undo.RecordObject(this, "Create new Node");
 
            switch (type)
            {
                case QuestNodeType.StartNode:
                    QuestStartNodeData squestdata = new QuestStartNodeData();
                    squestdata.UID = Guid.NewGuid().ToString();
                    startNodes.Add(squestdata);
                    nodeLookUp.Add(squestdata.UID, squestdata);
                    OnValidate();
                    return squestdata;
                    break;
                case QuestNodeType.DialogueNode:
                    return null;
                    break;
                default:
                    return null;
                    break;
            }
           

        }

      

        private void OnValidate()
        {
            //Debug.Log("Nodes: " + nodes);
            //nodeLookUp.Clear();
            //foreach (QuestNode _node in Nodes)
            //{
            //    nodeLookUp[_node.UID] = _node;
            //}   

        }

    }

}