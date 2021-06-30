﻿using System.Collections;
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
        Rect rect = new Rect(0,0,200,140);
        [HideInInspector]
        [SerializeField]
        Rect textFieldRect = new Rect(0, 0, 160, 18);

        [HideInInspector]
        [SerializeField]
        bool isEndPoint = false;
        [HideInInspector]
        [SerializeField]
        string endPointDescription;

        [SerializeField]  QuestVariableTemplate selectedVariable = null;
        [SerializeField]  int selectedOptionIndex = 0;
        [SerializeField] string requiredVarialbeValue;
        [SerializeField]  bool isUsingCondition = false;

        [SerializeField] bool checkInventory = false;
        [SerializeField] LootItem lootItem;
        [SerializeField] int requiredLootCount = 0;

        public Rect Rect { get => rect; set => rect = value; }
        public List<string> Children { get => children; private set => children = value; }
        public string UniqueID { get => uniqueID; set => uniqueID = value; }
        public string Text {
            get => text;
            set {
                if (text != value)
                {
                    //EditorUtility.SetDirty(selectedDialogue);   // update the Scriptable Object on Save
#if UNITY_EDITOR
                    Undo.RecordObject(this, "Update Dialog Text"); // automaticaly marks Scriptable Object  as dirty, records step for undo
                    text = value;
                    EditorUtility.SetDirty(this);// When Scriptable Object is in Asset file "setDirty" musst call seperated
#endif
                }
            }
        }

        public bool IsPlayerSpeaking
        {
            get => isPlayerSpeaking;
            set
            {
#if UNITY_EDITOR
                Undo.RecordObject(this, "Change Dialogue Speaker");
                isPlayerSpeaking = value;
                EditorUtility.SetDirty(this);
#endif
            }
        }

        public Rect TextFieldRect { get => textFieldRect;
            set {
                textFieldRect = value;
                //rect.height = stdRect.height + textFieldRect.height;
                //Debug.Log("Rect" + textFieldRect);
            }
        }

        public bool IsEndPoint { get => isEndPoint; set => isEndPoint = value; }
        public string EndPointDescription { get => endPointDescription; set => endPointDescription = value; }
        public float contentHeight { get; set; }
        public QuestVariableTemplate SelectedVariable { get => selectedVariable; set => selectedVariable = value; }
        public int SelectedOptionIndex { get => selectedOptionIndex; set => selectedOptionIndex = value; }
        public string RequiredVarialbeValue { get => requiredVarialbeValue; set => requiredVarialbeValue = value; }
        public bool IsUsingCondition { get => isUsingCondition; set => isUsingCondition = value; }
        public bool CheckInventory { get => checkInventory; set => checkInventory = value; }
        public LootItem LootItem { get => lootItem; set => lootItem = value; }
        public int RequiredLootCount { get => requiredLootCount; set => requiredLootCount = value; }

        public Rect GetTextFieldRectWithOffset()
        {  
            return new Rect(textFieldRect.x + rect.x, textFieldRect.y + rect.y, textFieldRect.width, textFieldRect.height);
        }


        public bool IsInventoyValid()
        {
            if (!CheckInventory) return true;

            return  Inventory.Instance.CheckForItem(lootItem, requiredLootCount, false);
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

    [System.Serializable]
    public class DialogueEndPoint
    {
        public string id;
        public string description;

        public DialogueEndPoint(string id, string description)
        {
            this.id = id;
            this.description = description;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DialogueNode))]
    public class DialogueNode_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // for other non-HideInInspector fields

            DialogueNode script = (DialogueNode)target;

            if(script.Children.Count == 0)
            {
                script.IsEndPoint = EditorGUILayout.Toggle("Is End Point", script.IsEndPoint);
            }

            if (script.IsEndPoint)
            {
                script.EndPointDescription = EditorGUILayout.TextField("EndPointDescription: ", script.EndPointDescription);
            }
        }
    }
#endif
}
