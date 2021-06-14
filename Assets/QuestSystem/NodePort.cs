using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace QuestSystem
{
    public delegate void OnClickNodePortDelegate(NodePort port, ConnectionPointType type);

    [System.Serializable]
    public class NodePort
    {
        private Rect rect;
        private float y_position;
        private ConnectionPointType type;
        PortPosition portPosition;
        PortSegment segment;
        private GUIStyle style;

        //private Action<NodePort> OnClickNodePort; // delegate function 
        public OnClickNodePortDelegate OnClickNodePort;


        public NodePort(PortSegment _segment, ConnectionPointType type, PortPosition _portPosition, OnClickNodePortDelegate OnClickNodePort )
        {
            this.Segment = _segment;
            this.portPosition = _portPosition;
            this.type = type;
            this.OnClickNodePort = OnClickNodePort;
            Rect = new Rect(0, 0, 14f, 14f);
            this.y_position = (_segment.CalcRect.height * 0.5f); //(y_position.HasValue) ? (node.Rect.height * 0.5f) : (float)y_position;
            StyleNodePort(type);
        }

        public Rect Rect { get => rect; private set => rect = value; }
        public PortSegment Segment { get => segment; private set => segment = value; }
        public ConnectionPointType Type { get => type;}
        public PortPosition PortPosition { get => portPosition; }

        private void StyleNodePort(ConnectionPointType type)
        {
            style = new GUIStyle();

            switch (type)
            {
                case ConnectionPointType.MainIn:
                case ConnectionPointType.MainOut:
                    style.normal.background = Resources.Load("port_main") as Texture2D;
                    style.active.background = Resources.Load("port_main_pressed") as Texture2D;
                    break;
                case ConnectionPointType.ReqIn:
                case ConnectionPointType.ReqOut:
                    style.normal.background = Resources.Load("port_requirement") as Texture2D;
                    style.active.background = Resources.Load("port_requirement_pressed") as Texture2D;
                    break;
                case ConnectionPointType.ActIn:
                case ConnectionPointType.ActOut:
                    style.normal.background = Resources.Load("port_action") as Texture2D;
                    style.active.background = Resources.Load("port_action_pressed") as Texture2D;
                    break;
                default:
                    break;
            }


        }

        public void Draw()
        {
            rect.y = Segment.CalcRect.y  + segment.CalcRect.height * 0.5f - Rect.height * 0.5f;

            switch (PortPosition)
            {
                case PortPosition.Left:
                    rect.x = Segment.CalcRect.x  - Rect.width - 1f;
                    break;

                case PortPosition.Right:
                    rect.x = Segment.CalcRect.x +  Segment.CalcRect.width + 1f;
                    break;
            }

            if (GUI.Button(Rect, "", style))
            {
                if (OnClickNodePort != null)
                {
                    OnClickNodePort(this, Type);
                }
            }
        }

    } 
}
