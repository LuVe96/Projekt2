using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{

    public delegate void QuestButtonPressed();

    public class QuestButton : MonoBehaviour
    {
        private QuestButtonPressed QuestButtonPressed = null;
        [SerializeField] GameObject questButtonObect;
        [SerializeField] Image img;
        [SerializeField] Button button;

        public static QuestButton Instance = null;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            button.onClick.AddListener(ButtonClicked);
        }

        private void ButtonClicked()
        {
           if(QuestButtonPressed != null)
            {
                QuestButtonPressed.Invoke();
            }
        }

        public void showButtonAsType(QuestButtonType type, QuestButtonPressed questButtonPressed)
        {
            QuestButtonPressed = questButtonPressed;
            questButtonObect.SetActive(true);
            switch (type)
            {
                case QuestButtonType.Talk: //set Images...
                    break;
                case QuestButtonType.Interact:
                    break;
                default:
                    break;
            }
        }

        public void HideQuestButton()
        {
            QuestButtonPressed = null;
            questButtonObect.SetActive(false);
        }
    }




    public enum QuestButtonType
    {
        Talk, Interact 
    }
}