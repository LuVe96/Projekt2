using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{
    public class PortSegment 
    {

        Dictionary<ConnectionPointType, NodePort> nodePortsDict = new Dictionary<ConnectionPointType, NodePort>();
        private Rect rect;
        private Node node;
        SegmentType type;

        public PortSegment(SegmentType _type, PortProps[] nodePorts, OnClickNodePortDelegate onClickNodePort, Node _node )
        {
            type = _type;
            Node = _node;
            foreach (PortProps pointType in nodePorts)
            {
                NodePortsDict.Add(pointType.connectionPointType, new NodePort(this, pointType.connectionPointType, pointType.portPosition, onClickNodePort));
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
        public SegmentType Type { get => type; }

        public void Begin()
        {
            //GUIStyle style = new GUIStyle();
            //style.normal.background = Texture2D.linearGrayTexture;

            rect = EditorGUILayout.BeginVertical();
        }

        public void End()
        {
            EditorGUILayout.Space();
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
