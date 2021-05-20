using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace QuestSystem
{
    public enum ConnectionPointType { In, Out }

    public class NodePort
    {
        private Rect rect;
        private float y_position;
        private ConnectionPointType type;
        private Node node;
        private GUIStyle style;

        private Action<NodePort> OnClickNodePort; // delegate function 

        public NodePort(Node node, ConnectionPointType type, GUIStyle style, Action<NodePort> OnClickNodePort)
        {
            this.Node = node;
            this.type = type;
            this.style = style;
            this.OnClickNodePort = OnClickNodePort;
            Rect = new Rect(0, 0, 10f, 20f);
            this.y_position = (node.Rect.height * 0.5f); //(y_position.HasValue) ? (node.Rect.height * 0.5f) : (float)y_position;
        }

        public Rect Rect { get => rect; private set => rect = value; }
        public Node Node { get => node; private set => node = value; }

        public void Draw()
        {
            rect.y = Node.Rect.y + y_position - Rect.height * 0.5f;

            switch (type)
            {
                case ConnectionPointType.In:
                    rect.x = Node.Rect.x - Rect.width + 8f;
                    break;

                case ConnectionPointType.Out:
                    rect.x = Node.Rect.x + Node.Rect.width - 8f;
                    break;
            }

            if (GUI.Button(Rect, "", style))
            {
                if (OnClickNodePort != null)
                {
                    OnClickNodePort(this);
                }
            }
        }

    } 
}
