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
        [SerializeField] List<QuestNode> nodes = new List<QuestNode>();
        [SerializeField] List<QuestStartNode> startNodes = new List<QuestStartNode>();
        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);
        Dictionary<string, QuestNode> nodeLookUp = new Dictionary<string, QuestNode>();

        public List<QuestNode> Nodes { get
            {
                List<QuestNode> allNodes = new List<QuestNode>();
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

        public IEnumerable<QuestNode> getAllNodes()
        {
            return Nodes;
        }

        public QuestNode CreateNewNode(QuestNodeType type)
        {

            Undo.RecordObject(this, "Create new Node");
 
            switch (type)
            {
                case QuestNodeType.StartNode:
                    QuestStartNode squestdata = new QuestStartNode();
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