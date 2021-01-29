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
        RunAction(currentQuest.questActions, QuestActionTime.End);
        currentQuest = null;
    }

    public void RunAction(QuestAction[] actions, QuestActionTime time)
    {
        foreach (var action in actions)
        {
            if( action.actionTime == time)
            {
                action.obj.transform.position = action.goalPosition;
            }
        }
    }


    public bool CanQuestStart(QuestDialogID id)
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

        if(quest.requiredQuestDialog == QuestDialogID.None)
        {
            currentQuest = quest;
            RunAction(currentQuest.questActions, QuestActionTime.Start);
            return true;
        }

        // check if requiredQuest of Quest already done
        foreach (var doneQ in doneQuests)
        {
            if (doneQ.dialogID == quest.requiredQuestDialog)
            {
                currentQuest = quest;
                RunAction(currentQuest.questActions, QuestActionTime.Start);
                return true;
            }
        }
        return false;

    }
}

[System.Serializable]
public class Quest
{
    public QuestDialogID dialogID;
    public QuestDialogID requiredQuestDialog;
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