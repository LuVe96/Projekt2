using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

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

        public NodePort(Node node, ConnectionPointType type, Action<NodePort> OnClickNodePort )
        {
            this.Node = node;
            this.type = type;
            this.OnClickNodePort = OnClickNodePort;
            Rect = new Rect(0, 0, 10f, 20f);
            this.y_position = (node.Rect.height * 0.5f); //(y_position.HasValue) ? (node.Rect.height * 0.5f) : (float)y_position;
            StyleNodePort(type);
        }

        public Rect Rect { get => rect; private set => rect = value; }
        public Node Node { get => node; private set => node = value; }

        private void StyleNodePort(ConnectionPointType type)
        {
            style = new GUIStyle();

            if(type == ConnectionPointType.In)
            {
                style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
                style.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
                style.border = new RectOffset(4, 4, 12, 12);
            } else
            {
                style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
                style.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
                style.border = new RectOffset(4, 4, 12, 12);
            }

        }

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
