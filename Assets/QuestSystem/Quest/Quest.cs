using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuestSystem.Quest
{
    //[CreateAssetMenu(fileName = "New Quest", menuName = "QuestSystem/Quest", order = 0)]
    public class Quest : MonoBehaviour
    {
        public string questName;
        [SerializeField] List<QuestNode> nodes = new List<QuestNode>();
        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);
        Dictionary<string, QuestNode> nodeLookUp = new Dictionary<string, QuestNode>();

        //Editordata
        //[SerializeField] List<Node> editorNodes = new List<Node>();
        //[SerializeField] List<NodeConnection> editorConnections = new List<NodeConnection>();


        //public List<Node> EditorNodes { get => editorNodes; set => editorNodes = value; }
        //public List<NodeConnection> EditorConnections { get => editorConnections; set => editorConnections = value; }
        public List<QuestNode> Nodes { get => nodes; set => nodes = value; }

        public List<string> testStrings = new List<string>();

        private void Start()
        {
            
        }

        public IEnumerable<QuestNode> getAllNodes()
        {
            return Nodes;
        }

        public QuestNode CreateNewNode()
        {
            QuestNode questdata = new QuestNode();
            questdata.UID = Guid.NewGuid().ToString();
            Nodes.Add(questdata);
            nodeLookUp.Add(questdata.UID, questdata);
            return questdata;
        }

        public void UpdateNodeData(QuestNode nodeData)
        {

            QuestNode node = nodes.FirstOrDefault(x => x.UID == nodeData.UID);
            if (node != null) node = nodeData;

        }

        private void OnValidate()
        {
            Debug.Log("Nodes: " + nodes);
            nodeLookUp.Clear();
            foreach (QuestNode _node in Nodes)
            {
                nodeLookUp[_node.UID] = _node;
            }
        }

    }

}