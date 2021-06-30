using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using QuestSystem.Quest;

namespace QuestSystem
{

    public delegate void RepaintEditorDelegate(bool resetup);

    public abstract class Node : ScriptableObject
    {
        private QuestNodeData questdata;
        private Rect rect;
        private bool isDragged;
        protected GUIStyle style;
        private float contentHeight;

        private GUIStyle headerStyle;
        private Rect headerRect;

        protected GUIStyle textStyle;
        protected GUIStyle rightPortTextStyle;
        protected GUIStyle leftPortTextStyle;
        protected GUIStyle headerTextStyle;
        protected GUIStyle headerButtonStyle;

        protected bool headerButtonPressed = false;

        List <KeyValuePair<SegmentType, PortSegment>> segments = new List<KeyValuePair<SegmentType, PortSegment>>();

        public Rect Rect { get => rect;
            private set {
                rect = value;
                headerRect.position = value.position;
                if (Questdata != null)
                {
                    Questdata.Rect = rect;
                }
            }
        }

        public QuestNodeData Questdata { get => questdata; set => questdata = value; }
        public List<KeyValuePair<SegmentType, PortSegment>> Segments { get => segments; set => segments = value; }

        protected RepaintEditorDelegate RepaintEditor;
        protected bool drawHeader = true;

        public Node(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
        {
            Init(_questdata, new Rect(position.x, position.y, width, height), OnClickNodePort, repaintEditorDelegate);
        }

        public Node(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
        {
            Init(_questdata , _questdata.Rect, OnClickNodePort, repaintEditorDelegate);
        }

        private void Init(QuestNodeData _questdata, Rect _rect, OnClickNodePortDelegate OnClickNodePort, RepaintEditorDelegate repaintEditorDelegate)
        {
            Questdata = _questdata;
            RepaintEditor = repaintEditorDelegate;

            Rect = _rect;
            _rect.height = 0;
            headerRect = _rect;
            style = UseNodeStyle();
            SetupStyles();
            SetupSegments(OnClickNodePort, segments);

        }

        private void SetupStyles()
        {
            textStyle = new GUIStyle();
            textStyle.normal.textColor = Color.white;
            textStyle.padding = new RectOffset(5, 0, 0, 5);
            textStyle.wordWrap = true;

            leftPortTextStyle = new GUIStyle(textStyle);
            leftPortTextStyle.fontStyle = FontStyle.Italic;

            rightPortTextStyle = new GUIStyle(leftPortTextStyle);
            rightPortTextStyle.alignment = TextAnchor.MiddleRight;

            headerTextStyle = new GUIStyle(textStyle);
            headerTextStyle.fontStyle = FontStyle.Bold;

            headerButtonStyle = new GUIStyle(headerTextStyle);
            var b = headerButtonStyle.border;
            b.left = 0;
            b.right = 0;
            b.top = 0;
            b.bottom = 0;
            headerButtonStyle.border = b;

            headerStyle = new GUIStyle();
            headerStyle.normal.background = Resources.Load("node_header") as Texture2D;
            headerStyle.border = new RectOffset(20, 20, 20, 20);
            headerStyle.padding = new RectOffset(24, 24, 16, 16);

        }

        protected abstract void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments);
        
        protected virtual GUIStyle UseNodeStyle()
        {
            style = new GUIStyle();
            //style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            style.normal.background = Resources.Load("node_blue") as Texture2D;
            style.border = new RectOffset(20, 20, 20, 20);
            style.padding = new RectOffset(24, 24, 16, 16);
            return style;
        }

        public void Drag(Vector2 delta)
        {
            Rect newRect = Rect;
            newRect.position += delta;
            Rect = newRect;
        }

#if UNITY_EDITOR
        public void Draw() {
            GUILayout.BeginArea(Rect, style);
            contentHeight = EditorGUILayout.BeginVertical().height;
            GUILayout.Space(headerRect.height);
            DrawContent();
            EditorGUILayout.EndHorizontal();

            GUILayout.EndArea();
            if (drawHeader)
            {
                DrawHeader();
            }

            DrawPorts();

            if (contentHeight != 0)
            {

                if ((contentHeight + 35) != Rect.height)
                {
                    Rect r = Rect;
                    r.height = contentHeight + 35;
                    Rect = r;
                    RepaintEditor(false);
                }
            }
        }

        private void DrawHeader()
        {
            GUILayout.BeginArea(headerRect, headerStyle);

            float headerHeight = EditorGUILayout.BeginVertical().height;
            DrawNodeHeader();
            GUILayout.Space(30);

            EditorGUILayout.EndHorizontal();

            GUILayout.EndArea();

            if (headerHeight != 0)
            {
                if ((headerHeight) != headerRect.height)
                {
                    Rect r = headerRect;
                    r.height = headerHeight;
                    headerRect = r;
                    RepaintEditor(false);
                }
            }
        }

        protected abstract void DrawContent();
        protected abstract void DrawNodeHeader();


        protected void DrawEditabelHeader(string defaultTilte)
        {
            MainNodeData data = Questdata as MainNodeData;

            if (!headerButtonPressed)
            {
                if (GUILayout.Button((data.Title != null && data.Title != "") ? data.Title : defaultTilte, headerTextStyle))
                {
                    headerButtonPressed = true;
                }
            }
            else
            {
                GUILayout.BeginHorizontal();
                data.Title = GUILayout.TextField(data.Title != null ? data.Title : defaultTilte);
                if (GUILayout.Button("✓", GUILayout.Width(25)))
                {
                    headerButtonPressed = false;
                }
                GUILayout.EndHorizontal();
            }
        }

        private void DrawPorts()
        {
            foreach (var segment in segments)
            {
                segment.Value.DrawPorts();
            }
        }

        public void AddChildsToData(Node childNode, PortSegment segment)
        {
            switch (segment.Type)
            {
                case SegmentType.MainSegment:
                    (Questdata as MainNodeData).ChildrenIDs.Add(childNode.Questdata.UID);
                    break;
                case SegmentType.RequirementSegment:
                    (Questdata as MainNodeData).RequirementIDs.Add(childNode.Questdata.UID);
                    break;
                case SegmentType.ActionSegment:
                    (Questdata as MainNodeData).ActionIDs.Add(childNode.Questdata.UID);
                    break;
                case SegmentType.DialogueEndPointSegment:
                    (Questdata as EndpointMainNodeData).AddChildToEndPoint((segment as EndPortSegment).EndPortId, childNode.Questdata.UID);
                    break;
                default:
                    break;
            }
        }

        public void RemoveChildsInData(Node childNode, PortSegment segment)
        {
            switch (segment.Type)
            {
                case SegmentType.MainSegment:
                    (Questdata as MainNodeData).ChildrenIDs.Remove(childNode.Questdata.UID);
                    break;
                case SegmentType.RequirementSegment:
                    (Questdata as MainNodeData).RequirementIDs.Remove(childNode.Questdata.UID);
                    break;
                case SegmentType.ActionSegment:
                    (Questdata as MainNodeData).ActionIDs.Remove(childNode.Questdata.UID);
                    break;
                case SegmentType.DialogueEndPointSegment:
                    if(childNode != null)
                    {
                        (Questdata as QuestDialogueNodeData).RemoveChildToFromPoint((segment as EndPortSegment).EndPortId, childNode.Questdata.UID);
                    } else
                    {
                        (Questdata as QuestDialogueNodeData).RemoveChildToFromPoint((segment as EndPortSegment).EndPortId, null);
                    }
                   
                    break;
                default:
                    break;
            }
        }

        public bool ProcessEvents(Event e, Vector2 scrollPosition)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (Rect.Contains(e.mousePosition + scrollPosition))
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
#endif
    }
}
