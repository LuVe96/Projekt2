using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{
    public class VariableActionNode : Node
    {
        public VariableActionNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public VariableActionNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public VariableActionData VariableActionData { get => (VariableActionData)Questdata; set { } }

        PortSegment actionSegment;
        QuestVariableTemplate selectedVariable = null;
        int selectedOptionIndex = 0;

        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {
            PortProps[] actionTypes = { new PortProps(ConnectionPointType.ActOut, PortPosition.Left) };
            actionSegment = new PortSegment(SegmentType.ActionSegment, actionTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(actionSegment.Type, actionSegment));
        }
        protected override GUIStyle UseNodeStyle()
        {
            style = new GUIStyle();
            //style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node3.png") as Texture2D;
            style.normal.background = Resources.Load("node_green") as Texture2D;
            style.border = new RectOffset(20, 20, 20, 20);
            style.padding = new RectOffset(24, 24, 16, 16);
            return style;
        }

        protected override void DrawNodeHeader()
        {
            EditorGUILayout.LabelField("Variable Action", headerTextStyle);
            GUILayout.Space(10);
            actionSegment.Begin();
            actionSegment.End();
        }

        protected override void DrawContent()
        {
            EditorGUILayout.LabelField("Variable Action", headerTextStyle);
            actionSegment.Begin();
            actionSegment.End();

            QuestVariableObject qvo = Resources.Load("QuestVariables") as QuestVariableObject;
            List<QuestVariableTemplate> variables = qvo.GetAllQuestVariableTemplates();
            int varIndex = variables.IndexOf(selectedVariable);

            if(variables.Count <= 0)
            {
                EditorGUILayout.LabelField("No Variables available");
            }
            else
            {
                GUILayout.Label("Set Variable:");
                GUILayout.BeginHorizontal();
                varIndex = EditorGUILayout.Popup(varIndex != -1 ? varIndex : 0, variables.Select(x => x.Title).ToArray());
                selectedVariable = variables[varIndex];
                VariableActionData.VariableName = selectedVariable.Title;

                if (selectedVariable != null)
                {
                    selectedOptionIndex = selectedVariable.Datas.IndexOf(VariableActionData.VariableValue);
                    selectedOptionIndex = EditorGUILayout.Popup(selectedOptionIndex != -1 ? selectedOptionIndex : 0, selectedVariable.Datas.ToArray());
                    VariableActionData.VariableValue = selectedVariable.Datas[selectedOptionIndex];
                    selectedOptionIndex = 0;
                }

            }

            GUILayout.EndHorizontal();
        }
    }

}