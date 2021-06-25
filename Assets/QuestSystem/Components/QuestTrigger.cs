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

        public static event Action OnTriggerChanged;

        public bool CheckTriggerState()
        {
            return isTriggerd;
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if(other.tag == onTriggerTag)
            {
                isTriggerd = true;
                OnTriggerChanged.Invoke();
            }

        }
    }

}