using QuestSystem.Quest;
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

            public VariableRequireData VariableRequireData { get => (VariableRequireData)Questdata; set { } }

            PortSegment requirementSegment;
            QuestVariableTemplate selectedVariable = null;
            int selectedOptionIndex = 0;

            protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
            {

                PortProps[] reqTypes = { new PortProps(ConnectionPointType.ReqOut, PortPosition.Right) };
                requirementSegment = new PortSegment(SegmentType.RequirementSegment, reqTypes, OnClickNodePort, this);
                segments.Add(new KeyValuePair<SegmentType, PortSegment>(requirementSegment.Type, requirementSegment));
            }

        protected override void DrawContent()
            {
                EditorGUILayout.LabelField("Variable Requirement", headerTextStyle);
                requirementSegment.Begin();
                requirementSegment.End();

                QuestVariableObject qvo = Resources.Load("QuestVariables") as QuestVariableObject;
                List<QuestVariableTemplate> variables = qvo.GetAllQuestVariableTemplates();
                int varIndex = variables.IndexOf(selectedVariable);

                if (variables.Count <= 0)
                {
                    EditorGUILayout.LabelField("No Variables available");
                }
                else
                {
                    GUILayout.Label("Check Variable:");
                    GUILayout.BeginHorizontal();
                    varIndex = EditorGUILayout.Popup(varIndex != -1 ? varIndex : 0, variables.Select(x => x.Title).ToArray());
                    selectedVariable = variables[varIndex];
                    VariableRequireData.VariableName = selectedVariable.Title;

                    if (selectedVariable != null)
                    {
                        selectedOptionIndex = selectedVariable.Datas.IndexOf(VariableRequireData.RequiredVarialbeValue);
                        selectedOptionIndex = EditorGUILayout.Popup(selectedOptionIndex != -1 ? selectedOptionIndex : 0, selectedVariable.Datas.ToArray());
                        VariableRequireData.RequiredVarialbeValue = selectedVariable.Datas[selectedOptionIndex];
                        selectedOptionIndex = 0;
                    }

                }

                GUILayout.EndHorizontal();
            }
        }

    }