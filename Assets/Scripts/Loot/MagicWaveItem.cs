using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MagicWaveItem", menuName = "Inventory/MagicWaveItem")]
public class MagicWaveItem : LootItem
{

    public OnHitEffectType onHitEffectType;
    public float effectTime;
    public float damageOverTime;
    public float walkSpeedMultiplier = 1;

    public override void UseItem()
    {
        GameObject.Find("Player").GetComponent<PlayerCombatHandler>().CastSpell(onHitEffectType);
        Inventory.Instance.Remove(this);
    }


}