using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HitEffectItem", menuName = "Inventory/HitEffectItem")]
public class HitEffectItem : LootItem
{

    public OnHitEffectType onHitEffectType;
    public float effectTime;
    public float effectItemDuration;
    public float damageOverTime;
    public float walkSpeedMultiplier = 1;

    public override void UseItem()
    {
        HitEffectManager.Instance.AddEffect(new OnHitEffect(onHitEffectType, effectTime, damageOverTime), effectItemDuration);
        Inventory.Instance.Remove(this);

    }

}
