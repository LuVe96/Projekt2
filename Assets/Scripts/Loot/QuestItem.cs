using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New QuestItem", menuName = "Inventory/QuestItem")]
public class QuestItem : LootItem
{

    public string endQuestId;

    public override void UseItem()
    {
        throw new System.NotImplementedException();
    }
}

