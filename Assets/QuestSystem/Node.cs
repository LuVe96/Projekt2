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
        protected private QuestNode questdata;
        private Rect rect;
        private bool isDragged;
        protected GUIStyle style;

        protected NodePort inPort;
        protected NodePort outPort;

        //public delegate void NodeChanged(QuestNode questNode);
        //protected NodeChanged NodeHasChanges;

        public Rect Rect { get => rect;
            private set {
                rect = value;
                if (questdata != null)
                {
                    questdata.Rect = rect;
                    //NodeHasChanges(questdata);
                }

            }
        }

        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, Action<NodePort> OnClickInPoint, Action<NodePort> OnClickOutPoint, QuestNode _questdata/*, NodeChanged _nodeChanged*/)
        {
            Init(nodeStyle, OnClickInPoint, OnClickOutPoint, _questdata/*, _nodeChanged*/, new Rect(position.x, position.y, width, height));
        }

        public Node( GUIStyle nodeStyle, Action<NodePort> OnClickInPoint, Action<NodePort> OnClickOutPoint, QuestNode _questdata/*, NodeChanged _nodeChanged*/)
        {
            Init(nodeStyle, OnClickInPoint, OnClickOutPoint, _questdata/*, _nodeChanged*/ , _questdata.Rect);
        }

        private void Init(GUIStyle nodeStyle, Action<NodePort> OnClickInPoint, Action<NodePort> OnClickOutPoint, QuestNode _questdata/*, NodeChanged _nodeChanged*/, Rect _rect)
        {
            //NodeHasChanges = new NodeChanged(_nodeChanged);
            questdata = _questdata;

            Rect = _rect;
            style = nodeStyle;

            inPort = new NodePort(this, ConnectionPointType.In, OnClickInPoint);
            outPort = new NodePort(this, ConnectionPointType.Out, OnClickOutPoint);
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
