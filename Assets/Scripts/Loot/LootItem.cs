using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName ="Inventory/Item")]
public abstract class LootItem : ScriptableObject
{
    new public string name;
    public Sprite icon;
    public GameObject gameObject;

    public abstract void UseItem();

}

public enum LootItemType {
    Healing,
    Scroll
}