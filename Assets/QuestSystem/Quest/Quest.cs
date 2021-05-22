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


        //Editordata
        //[SerializeField] List<Node> editorNodes = new List<Node>();
        //[SerializeField] List<NodeConnection> editorConnections = new List<NodeConnection>();


        //public List<Node> EditorNodes { get => editorNodes; set => editorNodes = value; }
        //public List<NodeConnection> EditorConnections { get => editorConnections; set => editorConnections = value; }
        //public List<QuestNode> Nodes { get => nodes; set => nodes = value; }

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

            //QuestNode questdata = new QuestNode();
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
            
            //questdata.UID = Guid.NewGuid().ToString();
            //Nodes.Add(questdata);
            //nodeLookUp.Add(questdata.UID, questdata);
            //return questdata;
        }

      

        private void OnValidate()
        {
            //Debug.Log("Nodes: " + nodes);
            //nodeLookUp.Clear();
            //foreach (QuestNode _node in Nodes)
            //{
            //    nodeLookUp[_node.UID] = _node;
            //}

            //startNodes.Clear();
            //foreach (QuestNode node in nodes)
            //{
            //    if (node is QuestStartNode)
            //    {
            //        QuestStartNode n = (QuestStartNode)node;
            //        Debug.Log("Val: " + n.testStartString);
            //        startNodes.Add(n);

            //    }
            //}


        }

    }

}