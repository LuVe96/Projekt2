using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace QuestSystem.Quest
{
    [CustomEditor(typeof(Quest))]
    public class QuestEditor : Editor
    {

        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            Quest quest = (Quest)target;

            if ( GUILayout.Button("Open Window"))
            {
                QuestEditorWindow window = (QuestEditorWindow)EditorWindow.GetWindow(typeof(QuestEditorWindow), false, "Quest Editor");
                window.Init(this, target);
            }
        }


    } 
}

