using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem.Dialogue
{

    public class DialogueNode : ScriptableObject
    {
        [HideInInspector]
        [SerializeField]
        private string uniqueID;
        [SerializeField]
        bool isPlayerSpeaking = false;
        [SerializeField]
        string text;
        [SerializeField]
        List<string> children = new List<string>();
        [SerializeField]
        Rect rect = new Rect(0,0,200,100);
        [HideInInspector]
        [SerializeField]
        Rect textFieldRect = new Rect(0, 0, 160, 18);

        //private Rect stdRect = new Rect(0, 0, 200, 100-18);

        public Rect Rect { get => rect; set => rect = value; }
        public List<string> Children { get => children; private set => children = value; }
        public string UniqueID { get => uniqueID; set => uniqueID = value; }
        public string Text {
            get => text;
            set {
                if (text != value)
                {
                    //EditorUtility.SetDirty(selectedDialogue);   // update the Scriptable Object on Save
                    Undo.RecordObject(this, "Update Dialog Text"); // automaticaly marks Scriptable Object  as dirty, records step for undo
                    text = value;
                    EditorUtility.SetDirty(this);// When Scriptable Object is in Asset file "setDirty" musst call seperated
                }
            }
        }

        public bool IsPlayerSpeaking
        {
            get => isPlayerSpeaking;
            set
            {
                Undo.RecordObject(this, "Change Dialogue Speaker");
                isPlayerSpeaking = value;
                EditorUtility.SetDirty(this);
            }
        }

        public Rect TextFieldRect { get => textFieldRect;
            set {
                textFieldRect = value;
                //rect.height = stdRect.height + textFieldRect.height;
                Debug.Log("Rect" + textFieldRect);
            }
        }

        public Rect GetTextFieldRectWithOffset()
        {  
            return new Rect(textFieldRect.x + rect.x, textFieldRect.y + rect.y, textFieldRect.width, textFieldRect.height);
        }

#if UNITY_EDITOR

        //private void Awake()
        //{
        //    stdRect = rect;
        //    stdRect.height -= textFieldRect.height;
        //}

        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string childId)
        {
            Undo.RecordObject(this, "Link Child");
            children.Add(childId);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childId)
        {
            Undo.RecordObject(this, "Unlink Child");
            children.Remove(childId);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
