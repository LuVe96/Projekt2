using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HealingItem", menuName = "Inventory/HealingItem")]
public class HealingItem : LootItem
{

    public float healing = 0;

    public override void UseItem()
    {
        GameObject.Find("Player").GetComponent<PlayerHandler>().HealPlayer(healing);
        Inventory.Instance.Remove(this);
    }


}
