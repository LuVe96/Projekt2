using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystemManager : MonoBehaviour
{

    [SerializeField] QuestSystem.Quest.Quest[] quests;


    // Start is called before the first frame update
    void Start()
    {
        foreach (QuestNodeData item in quests[0].getAllNodes())
        {
            item.execute();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
