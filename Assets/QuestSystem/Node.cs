using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace QuestSystem
{
    public class Node
    {
        private Rect rect;
        private string title;
        private bool isDragged;

        private GUIStyle style;

        private NodePort inPort;
        private NodePort outPort;

        public Rect Rect { get => rect; private set => rect = value; }

        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, Action<NodePort> OnClickInPoint, Action<NodePort> OnClickOutPoint)
        {
            Rect = new Rect(position.x, position.y, width, height);
            style = nodeStyle;

            inPort = new NodePort(this, ConnectionPointType.In, OnClickInPoint);
            outPort = new NodePort(this, ConnectionPointType.Out, OnClickOutPoint);
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public void Draw()
        {
            inPort.Draw();
            outPort.Draw();
            GUI.Box(Rect, title, style);
        }

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
