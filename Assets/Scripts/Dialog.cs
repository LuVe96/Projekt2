using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog 
{

    public DialogID dialogID;
    public string name;
    public Sprite charImage;
    public DialogSentence[] dialogSentences;

}

[System.Serializable]
public class DialogSentence
{
    public DialogParticipant dialogParticipant;
    [TextArea(3, 10)]
    public string sentence;
}

public enum DialogParticipant
{
    Player, NPC
}

public enum DialogID
{
    None, Zwerg_1, Zwerg_2, Zwerg_3,
}