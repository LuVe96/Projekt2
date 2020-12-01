using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName ="Inventory/Item")]
public class LootItem : ScriptableObject
{
    new public string name;
    public Sprite icon;
    public GameObject gameObject;

    public LootItemType type;
    public float healing = 0;

}

public enum LootItemType {
    Healing,
    Scroll
}