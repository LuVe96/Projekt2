using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    public Quest[] quests;

    private Quest currentQuest;
    private List<Quest> doneQuests = new List<Quest>();
    private List<string> achivedQuestEndIDs = new List<string>();


    public static QuestManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);  // the Singelton Obj gets not deleted when change szene
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ArchiveEndQuest(string questEndID)
    {
        achivedQuestEndIDs.Add(questEndID);
        if(currentQuest.requiredToEndID == questEndID)
        {
            EndQuest();
        }
    }

    public void EndQuest()
    {
        doneQuests.Add(currentQuest);
        currentQuest = null;
    }


    public bool CanQuestStart(DialogID id)
    {

        // check if Quest already done
        foreach (var doneQ in doneQuests)
        {
            if( doneQ.dialogID == id)
            {
                return false;
            }      
        }

        Quest quest = null;


        foreach (var _quest in quests)
        {
            if (_quest.dialogID == id)
            {
                quest = _quest;
            }
        }

        if(quest == null) { return false; };

        // check if requiredQuest of Quest already done
        foreach (var doneQ in doneQuests)
        {
            if (doneQ.dialogID == quest.requiredDialog)
            {
                currentQuest = quest;
                return true;
            }
        }

        return false;

    }
}

[System.Serializable]
public class Quest
{
    public DialogID dialogID;
    public DialogID requiredDialog;
    public string requiredToEndID = null;
    public QuestAction[] questActions;
}

[System.Serializable]
public class QuestAction
{
    public GameObject obj;
    public Vector3 goalPosition;
    public QuestActionTime actionTime;
}

public enum QuestActionTime
{
    Start, End
}