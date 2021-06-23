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
        public Color LineColor { get
            {
                Color color = Color.white;
                switch (type)
                {

                    case SegmentType.DialogueEndPointSegment:
                    case SegmentType.MainSegment:
                        ColorUtility.TryParseHtmlString("#55C6F2", out color);
                        return color;
                    case SegmentType.RequirementSegment:
                        ColorUtility.TryParseHtmlString("#F2B979", out color);
                        return color;
                    case SegmentType.ActionSegment:
                        ColorUtility.TryParseHtmlString("#85F297", out color);
                        return color;
                    default:
                        return color;
                }
            }
        }

        public void Begin()
        {
            //GUIStyle style = new GUIStyle();
            //style.normal.background = Texture2D.linearGrayTexture;

            rect = EditorGUILayout.BeginVertical();
            GUILayout.Space(5);
        }

        public void End()
        {
            //EditorGUILayout.Space();
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
