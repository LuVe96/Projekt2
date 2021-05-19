using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace QuestSystem.Quest
{
    public class QuestEditorWindow : EditorWindow
    {
        static Quest currentQuest = null;

        [MenuItem("Tools/QuestWindow")]
        public static void Init()
        {
            EditorWindow.GetWindow(typeof(QuestEditorWindow), false, "Editor Window");
        }

        internal void Init(QuestEditor questEditor, UnityEngine.Object target)
        {
            Init();
            currentQuest = (Quest)target;
        }

        private void OnGUI()
        {
            if(currentQuest == null)
            {
                EditorGUILayout.LabelField("Select A Quest");
            }
            else
            {
                EditorGUILayout.LabelField("Name: " + currentQuest.questName);
                currentQuest.questName = EditorGUILayout.TextField("Name: ", currentQuest.questName);
            }



        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

        }

        private void OnSelectionChanged()
        {
            Quest quest = Selection.activeGameObject.GetComponent<Quest>() as Quest;
            Debug.Log("Selectd: " + Selection.activeGameObject);
            if (quest != null)
            {
                currentQuest = quest;
                Repaint();
            }
        }
    } 
}
