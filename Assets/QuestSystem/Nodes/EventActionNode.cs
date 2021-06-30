using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{

        
    public class EventActionNode : Node
    {
        public EventActionNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public EventActionNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public EventActionData EventActionData { get => (EventActionData)Questdata; set { } }

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
            GUILayout.Label("Event Action", headerTextStyle);
            actionSegment.Begin();
            actionSegment.End();
        }

        MonoBehaviour mono;
        GameObject gameObject;
        MethodInfo[] methodes;
        MonoBehaviour[] monoBehaviours;

        int methodeIndex = 0;
        int monoIndex = 0;
        string[] n = { "none" };

        protected override void DrawContent()
        {


            GUILayout.Label("Function To Invoke:", textStyle);
            mono = EventActionData.MonoBehaviour;
            if(mono != null)
            {
                gameObject = mono.gameObject;
            }
            gameObject = (GameObject)EditorGUILayout.ObjectField(gameObject, typeof(GameObject), true);

            if( gameObject != null)
            {
                monoBehaviours = gameObject.GetComponents<MonoBehaviour>();
            }
            string[] mbs = monoBehaviours != null ? monoBehaviours.Select(m => m.GetType().Name).ToArray() : n;
            if (mono != null)
            {
                monoIndex = mbs.ToList().IndexOf(mono.GetType().Name);
            }
            monoIndex = EditorGUILayout.Popup( monoIndex, mbs.Length > 0 ? mbs : n);
            if(monoBehaviours != null)
            {
                if (monoBehaviours.Length > 0)
                {
                    mono = monoBehaviours[monoIndex];  
                }
            }

            if (mono != null)
            {
                methodes = mono.GetType().GetMethods().Where(m => m.IsPublic && m.ReturnType == typeof(void)
                && m.DeclaringType != typeof(Behaviour) && m.DeclaringType != typeof(MonoBehaviour)
                && m.DeclaringType != typeof(Component)).ToArray() ;
            }
            string[] methodStrings = methodes != null ? methodes.Select(m => m.Name).ToArray() : n;
            if (EventActionData.MethodeName != null)
            {
                methodeIndex = methodStrings.ToList().IndexOf(EventActionData.MethodeName);
            }
            methodeIndex = EditorGUILayout.Popup(methodeIndex, methodStrings.Length > 0 ? methodStrings : n);
            if (methodes != null)
            {
                if (methodes.Length > 0)
                {
                    EventActionData.MonoBehaviour = mono;
                    EventActionData.MethodeName = methodes[methodeIndex].Name;
                }
            }
        }
#endif

    }

}