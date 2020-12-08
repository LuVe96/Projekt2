using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUpItem", menuName = "Inventory/PowerUpItem")]
public class PowerUpItem : LootItem
{
    public override void UseItem()
    {
        Debug.LogWarning("Not Used");
    }
}
