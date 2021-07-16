using QuestSystem.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{
    public class VariableRequireNode : Node
    {
        public VariableRequireNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public VariableRequireNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public VariableRequireData Data { get => (VariableRequireData)Questdata; set { } }

        PortSegment requirementSegment;
        QuestVariableTemplate selectedVariable = null;
        int selectedOptionIndex = 0;

        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {

            PortProps[] reqTypes = { new PortProps(ConnectionPointType.ReqOut, PortPosition.Right) };
            requirementSegment = new PortSegment(SegmentType.RequirementSegment, reqTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(requirementSegment.Type, requirementSegment));
        }

        protected override GUIStyle UseNodeStyle()
        {
            style = new GUIStyle();
            style.normal.background = Resources.Load("node_red") as Texture2D;
            style.border = new RectOffset(20, 20, 20, 20);
            style.padding = new RectOffset(24, 24, 16, 16);
            return style;
        }
#if UNITY_EDITOR
        protected override void DrawNodeHeader()
        {
            EditorGUILayout.LabelField("Variable Requirement", headerTextStyle);
            requirementSegment.Begin();
            requirementSegment.End();
        }

        protected override void DrawContent()
        {

            QuestVariableObject qvo = Resources.Load("QuestVariables") as QuestVariableObject;
            List<QuestVariableTemplate> variables = qvo.GetAllQuestVariableTemplates();
            int varIndex = variables.IndexOf(variables.Find(v => v.Title == Data.VariableName));

            if (variables.Count <= 0)
            {
                GUILayout.Label("No Variables available", textStyle);
            }
            else 
            {
                GUILayout.Label("Check Variable:", textStyle);

                varIndex = EditorGUILayout.Popup(varIndex != -1 ? varIndex : 0, variables.Select(x => x.Title).ToArray());
                selectedVariable = variables[varIndex];
                Data.VariableName = selectedVariable.Title;

                GUILayout.Label("Value:", textStyle);
                if (selectedVariable != null)
                {          

                    if (selectedVariable.Type == QuestVariableType.Bool)
                    {
                        DrawVariableValueSelection();
                        Data.GetterType = VariableGetterType.equals;
                    }
                    else if (selectedVariable.Type == QuestVariableType.State)
                    {
                        DrawVariableEnumArea();
                    }
                }

            }


        }

        private void DrawVariableValueSelection()
        {
            selectedOptionIndex = selectedVariable.Datas.IndexOf(Data.RequiredVarialbeValue);
            selectedOptionIndex = EditorGUILayout.Popup(selectedOptionIndex != -1 ? selectedOptionIndex : 0, selectedVariable.Datas.ToArray());
            Data.RequiredVarialbeValue = selectedVariable.Datas[selectedOptionIndex];
            selectedOptionIndex = 0;
        }

        private void DrawVariableEnumArea()
        {
            GUILayout.BeginHorizontal();
            Data.GetterType = (VariableGetterType)EditorGUILayout.EnumPopup(Data.GetterType);
            DrawVariableValueSelection();

            GUILayout.EndHorizontal();
        }
#endif

    }
    public enum VariableGetterType
    {
        equals, heigherThan, lowerThan
    }
}