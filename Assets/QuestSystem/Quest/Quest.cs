﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem.Quest
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "QuestSystem/Quest", order = 0)]
    public class Quest : MonoBehaviour
    {
        public string questName;
        [SerializeField] List<QuestNode> nodes = new List<QuestNode>();
        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);
        Dictionary<string, QuestNode> nodeLookUp = new Dictionary<string, QuestNode>();

        private void Start()
        {
            
        }

        public IEnumerable<QuestNode> getAllNodes()
        {
            return nodes;
        }

    }
}