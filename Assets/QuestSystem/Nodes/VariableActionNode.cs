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

        public VariableActionData Data { get => (VariableActionData)Questdata; set { } }

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
#if UNITY_EDITOR
        protected override void DrawNodeHeader()
        {
            GUILayout.Label("Variable Action", headerTextStyle);
            actionSegment.Begin();
            actionSegment.End();
        }

        protected override void DrawContent()
        {


            QuestVariableObject qvo = Resources.Load("QuestVariables") as QuestVariableObject;
            List<QuestVariableTemplate> variables = qvo.GetAllQuestVariableTemplates();
            int varIndex = variables.IndexOf(variables.Find(v => v.Title == Data.VariableName));

            if(variables.Count <= 0)
            {
                GUILayout.Label("No Variables available", textStyle);
            }
            else 
            {
                GUILayout.Label("Set Variable:", textStyle);
                //GUILayout.BeginHorizontal();
                varIndex = EditorGUILayout.Popup(varIndex != -1 ? varIndex : 0, variables.Select(x => x.Title).ToArray());
                selectedVariable = variables[varIndex];
                Data.VariableName = selectedVariable.Title;

                GUILayout.Label("Value:", textStyle);
                if (selectedVariable != null)
                {
                    if(selectedVariable.Type == QuestVariableType.Bool)
                    {
                        DrawVariableValueSelection();
                        Data.SetterType = VariableSetterType.setTo;
                    }
                    else if( selectedVariable.Type == QuestVariableType.State)
                    {
                        DrawVariableEnumArea();
                    }
                   
                }

                //GUILayout.EndHorizontal();
            }

        }

        private void DrawVariableEnumArea()
        {
            GUILayout.BeginHorizontal();
            Data.SetterType = (VariableSetterType) EditorGUILayout.EnumPopup(Data.SetterType);

            if(Data.SetterType == VariableSetterType.setTo)
            {
                DrawVariableValueSelection();
            }
            else
            {
                Data.Steps = EditorGUILayout.IntField(Data.Steps);
            }


            GUILayout.EndHorizontal();
        }

        private void DrawVariableValueSelection()
        {
            selectedOptionIndex = selectedVariable.Datas.IndexOf(Data.VariableValue);
            selectedOptionIndex = EditorGUILayout.Popup(selectedOptionIndex != -1 ? selectedOptionIndex : 0, selectedVariable.Datas.ToArray());
            Data.VariableValue = selectedVariable.Datas[selectedOptionIndex];
            selectedOptionIndex = 0;
        }
#endif
    }

    public enum VariableSetterType
    {
        setTo, increaseSteps, decreaseSteps
    }

}