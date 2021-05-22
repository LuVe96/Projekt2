using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using QuestSystem.Quest;

namespace QuestSystem
{

    public abstract class Node : ScriptableObject
    {
        private QuestNodeData questdata;
        private Rect rect;
        private bool isDragged;
        protected GUIStyle style;

        private NodePort inPort;
        private NodePort outPort;

        public Rect Rect { get => rect;
            private set {
                rect = value;
                if (Questdata != null)
                {
                    Questdata.Rect = rect;
                }

            }
        }

        public QuestNodeData Questdata { get => questdata; set => questdata = value; }
        public NodePort OutPort { get => outPort; set => outPort = value; }
        public NodePort InPort { get => inPort; set => inPort = value; }

        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, Action<NodePort> OnClickInPoint, Action<NodePort> OnClickOutPoint, QuestNodeData _questdata)
        {
            Init(nodeStyle, OnClickInPoint, OnClickOutPoint, _questdata, new Rect(position.x, position.y, width, height));
        }

        public Node( GUIStyle nodeStyle, Action<NodePort> OnClickInPoint, Action<NodePort> OnClickOutPoint, QuestNodeData _questdata)
        {
            Init(nodeStyle, OnClickInPoint, OnClickOutPoint, _questdata , _questdata.Rect);
        }

        private void Init(GUIStyle nodeStyle, Action<NodePort> OnClickInPoint, Action<NodePort> OnClickOutPoint, QuestNodeData _questdata, Rect _rect)
        {
            Questdata = _questdata;

            Rect = _rect;
            style = nodeStyle;

            InPort = new NodePort(this, ConnectionPointType.In, OnClickInPoint);
            OutPort = new NodePort(this, ConnectionPointType.Out, OnClickOutPoint);
        }

        public void Drag(Vector2 delta)
        {
            Rect newRect = Rect;
            newRect.position += delta;
            Rect = newRect;
        }

        public abstract void Draw();

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (Rect.Contains(e.mousePosition))
                        {
                            isDragged = true;
                            GUI.changed = true;
                        }
                        else
                        {
                            GUI.changed = true;
                        }
                    }
                    break;

                case EventType.MouseUp:
                    isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }

            return false;
        }
    } 
}
