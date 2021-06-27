using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestTrigger : MonoBehaviour
    {
        [SerializeField] string onTriggerTag = "Player";
        bool isTriggerd = false;

        public event Action OnTriggerChanged;

        public bool CheckTriggerState()
        {
            return isTriggerd;
        }

        public void Check()
        {
            Debug.Log("Event check");
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if(other.tag == onTriggerTag)
            {
                isTriggerd = true;
                if(OnTriggerChanged != null)
                OnTriggerChanged.Invoke();
            }

        }
    }

}