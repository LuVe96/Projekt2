using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{
    public class NodeSegment 
    {

        Dictionary<ConnectionPointType, NodePort> nodePortsDict = new Dictionary<ConnectionPointType, NodePort>();
        private Rect rect;
        private Node node;

        public NodeSegment(ConnectionPointType[] nodePorts, OnClickNodePortDelegate onClickNodePort, Node _node )
        {
            Node = _node;
            foreach (ConnectionPointType pointType in nodePorts)
            {
                NodePortsDict.Add(pointType, new NodePort(this, pointType, onClickNodePort));
            }
        }

        public Rect CalcRect { get
            {
                Rect cR = rect;
                cR.position += Node.Rect.position;
                return cR;
            }
        }
        public Node Node { get => node; set => node = value; }
        public Dictionary<ConnectionPointType, NodePort> NodePortsDict { get => nodePortsDict; set => nodePortsDict = value; }

        public void Begin()
        {
            GUIStyle style = new GUIStyle();
            style.normal.background = Texture2D.linearGrayTexture;

            rect = EditorGUILayout.BeginVertical(style);
           


        }

        public void End()
        {
            EditorGUILayout.EndVertical();
            
        }

        public void DrawPorts()
        {
            foreach (NodePort port in NodePortsDict.Values)
            {
                port.Draw();
            }
        }
    } 
}
