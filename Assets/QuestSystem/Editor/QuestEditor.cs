using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace QuestSystem.Quest
{
    [CustomEditor(typeof(Quest))]
    public class QuestEditor : Editor
    {


        bool showAddName = false;
        string qName = ""; 
        string qLogName = "";

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            Quest quest = (Quest)target;

            QuestStateObject qso = Resources.Load("QuestStateData") as QuestStateObject;

            QuestName[] choices = qso.GetAllQuestNames().ToArray();
            int selectedIndex = -1;
            if(qName != "" && !showAddName)
            {
                selectedIndex = qso.GetAllQuestNames().Select(qs => qs.Name).ToList().IndexOf(qName);
                qName = "";
            }
            if (selectedIndex == -1)
            {
                selectedIndex = qso.GetAllQuestNames().Select(qs => qs.Name).ToList().IndexOf(quest.QuestName.Name);
            }
            GUILayout.BeginHorizontal();
            selectedIndex = EditorGUILayout.Popup(selectedIndex != -1 ? selectedIndex : 0, choices.Select(qs => qs.Name).ToArray());
            quest.QuestName = choices[selectedIndex];
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                showAddName = true;
            }

            GUILayout.EndHorizontal();

            if (showAddName)
            {

                qName = EditorGUILayout.TextField("QuestName", qName);
                qLogName = EditorGUILayout.TextField("QuestLogName", qLogName);

                if (GUILayout.Button("✓", GUILayout.Width(30)))
                {
                    qso.AddQuestName(qName, qLogName);
                    EditorUtility.SetDirty(qso);
                    showAddName = false;                    
                }

            }


            if ( GUILayout.Button("Open Window"))
            {
                QuestEditorWindow window = (QuestEditorWindow)EditorWindow.GetWindow(typeof(QuestEditorWindow), false, "Quest Editor");
                window.Init(this, target);
            }
        }


    } 
}

