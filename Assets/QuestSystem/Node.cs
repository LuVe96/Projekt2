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

        List <KeyValuePair<SegmentType, NodeSegment>> segments = new List<KeyValuePair<SegmentType, NodeSegment>>();

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
        public List<KeyValuePair<SegmentType, NodeSegment>> Segments { get => segments; set => segments = value; }


        //public NodePort OutPort { get => outPort; set => outPort = value; }
        //public NodePort InPort { get => inPort; set => inPort = value; }

        public Node(Vector2 position, float width, float height, QuestNodeData _questdata)
        {
            Init(_questdata, new Rect(position.x, position.y, width, height));
        }

        public Node( QuestNodeData _questdata)
        {
            Init(_questdata , _questdata.Rect);
        }

        private void Init(QuestNodeData _questdata, Rect _rect)
        {
            Questdata = _questdata;

            Rect = _rect;
            style = UseStyle();

            //InPort = new NodePort(this, ConnectionPointType.In, OnClickInPoint);
            //OutPort = new NodePort(this, ConnectionPointType.Out, OnClickOutPoint);
        }

        protected virtual GUIStyle UseStyle()
        {
            style = new GUIStyle();
            style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            style.border = new RectOffset(20, 20, 20, 20);
            style.padding = new RectOffset(15, 15, 15, 15);
            return style;
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
