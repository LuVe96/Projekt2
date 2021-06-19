using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace QuestSystem.Quest
{
    [CustomEditor(typeof(Quest))]
    public class QuestEditor : Editor
    {


        bool showAddName = false;
        string name = "";

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            Quest quest = (Quest)target;

            QuestStateObject qso = Resources.Load("QuestStateData") as QuestStateObject;



            string[] choices = qso.GetAllQuestNames().ToArray();
            int selectedIndex = -1;
            if(name != "" && !showAddName)
            {
                selectedIndex = qso.GetAllQuestNames().IndexOf(name);
                name = "";
            }
            if (selectedIndex == -1)
            {
                selectedIndex = qso.GetAllQuestNames().IndexOf(quest.QuestName);
            }
            GUILayout.BeginHorizontal();
            selectedIndex = EditorGUILayout.Popup(selectedIndex != -1 ? selectedIndex : 0, choices);
            quest.QuestName = choices[selectedIndex];
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                showAddName = true;
            }

            GUILayout.EndHorizontal();

            if (showAddName)
            {
                GUILayout.BeginHorizontal();
                name = EditorGUILayout.TextField(name);
                if (GUILayout.Button("✓", GUILayout.Width(30)))
                {
                    qso.AddQuestName(name);
                    showAddName = false;                    
                }
                GUILayout.EndHorizontal();
            }


            if ( GUILayout.Button("Open Window"))
            {
                QuestEditorWindow window = (QuestEditorWindow)EditorWindow.GetWindow(typeof(QuestEditorWindow), false, "Quest Editor");
                window.Init(this, target);
            }
        }


    } 
}

