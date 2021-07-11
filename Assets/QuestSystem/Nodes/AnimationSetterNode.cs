using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{
    public class AnimationSetterNode : Node
    {
        public AnimationSetterNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public AnimationSetterNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        private AnimationSetterData Data { get => (AnimationSetterData)Questdata; set { } }

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
            GUILayout.Label("Animation Setter", headerTextStyle);
            actionSegment.Begin();
            actionSegment.End();
        }

        protected override void DrawContent()
        {


            GUILayout.Label("CustomAnimations:", textStyle);
            Data.CustomAnimations = (CustomAnimations)EditorGUILayout.ObjectField(Data.CustomAnimations, typeof(CustomAnimations), true);

            GUILayout.Label("Clip:", textStyle);
            Data.Clip = (AnimationClip)EditorGUILayout.ObjectField(Data.Clip, typeof(AnimationClip), false);

            GUILayout.Label("For Type:", textStyle);
            Data.Type = (CustomAnimationStateType) EditorGUILayout.EnumPopup(Data.Type);
        }
#endif
    }
}