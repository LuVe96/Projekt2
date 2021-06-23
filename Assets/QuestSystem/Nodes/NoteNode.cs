using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{
    public class NoteNode : Node
    {
        public NoteNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
          : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
            drawHeader = false;
        }

        public NoteNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
            drawHeader = false;
        }

        GUIStyle textFieldStyle;

        public NoteNodeData NoteNodeData { get => (NoteNodeData)Questdata; set { } }

        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {
           // No Segemnt needed
        }

        protected override void DrawNodeHeader()
        {
        }

        protected override GUIStyle UseNodeStyle()
        {

            textFieldStyle = EditorStyles.textField;
            textFieldStyle.wordWrap = true;

            style = new GUIStyle();
            style.normal.background = Resources.Load("node_gray") as Texture2D;
            style.border = new RectOffset(20, 20, 20, 20);
            style.padding = new RectOffset(24, 24, 16, 16);
            return style;

        }

        bool noteButtnPressed = false;

        protected override void DrawContent()
        {
            //GUILayout.Label("Note", headerTextStyle);
            GUILayout.Space(10);

            if (!noteButtnPressed)
            {
                if (GUILayout.Button((NoteNodeData.Description != null && NoteNodeData.Description != "") ? NoteNodeData.Description : "Click here for note.", textStyle))
                {
                    noteButtnPressed = true;
                }
            }
            else
            {
                NoteNodeData.Description = GUILayout.TextField(NoteNodeData.Description != null ? NoteNodeData.Description : "", textFieldStyle);
                if (GUILayout.Button("✓", GUILayout.Width(25)))
                {
                    noteButtnPressed = false;
                }
            }
            GUILayout.Space(10);
        }

    }
}