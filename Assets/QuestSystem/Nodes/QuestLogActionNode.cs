using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{
    public class QuestLogActionNode : Node
    {
        public QuestLogActionNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public QuestLogActionNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public QuestLogActionData Data { get => (QuestLogActionData)Questdata; set { } }

        PortSegment actionSegment;

        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {
            PortProps[] actionTypes = { new PortProps(ConnectionPointType.ActOut, PortPosition.Left) };
            actionSegment = new PortSegment(SegmentType.ActionSegment, actionTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(actionSegment.Type, actionSegment));
        }

        protected override GUIStyle UseNodeStyle()
        {
            style = new GUIStyle();
            style.normal.background = Resources.Load("node_green") as Texture2D;
            style.border = new RectOffset(20, 20, 20, 20);
            style.padding = new RectOffset(24, 24, 16, 16);
            return style;
        }
#if UNITY_EDITOR
        protected override void DrawNodeHeader()
        {
            GUILayout.Label("Quest Log" , headerTextStyle);
            actionSegment.Begin();
            actionSegment.End();
        }


        protected override void DrawContent()
        {
            GUIStyle textFieldStyle = EditorStyles.textArea;
            textFieldStyle.wordWrap = true;

            GUILayout.Label("Log Text:", textStyle);
            Data.QuestLogText = GUILayout.TextArea(Data.QuestLogText, textFieldStyle, GUILayout.Height(50));
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("For Other Quest", textStyle);
            Data.ToOtherQuest = GUILayout.Toggle(Data.ToOtherQuest, "");
            GUILayout.EndHorizontal();

            if (Data.ToOtherQuest)
            {
                QuestStateObject qso = Resources.Load("QuestStateData") as QuestStateObject;

                QuestName[] choices = qso.GetAllQuestNames().ToArray();
                int selectedIndex = -1;
                if (selectedIndex == -1)
                {
                    selectedIndex = qso.GetAllQuestNames().Select(qs => qs.LogName).ToList().IndexOf(Data.QuestLogName);
                }
                GUILayout.BeginHorizontal();
                selectedIndex = EditorGUILayout.Popup(selectedIndex != -1 ? selectedIndex : 0, choices.Select(qs => qs.LogName).ToArray());
                Data.QuestLogName = choices[selectedIndex].LogName;
            }

        }
#endif
    }
}