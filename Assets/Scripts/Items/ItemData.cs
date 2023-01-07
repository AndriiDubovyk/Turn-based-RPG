using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewItemData", menuName = "SO/ItemData")]
public class ItemData : ScriptableObject
{
    public new string name;
    public string description;
    public int attack;
    public int defense;
    public int healing;

    public Sprite sprite;

    public ItemData Clone()
    {
        ItemData itemData = ScriptableObject.CreateInstance<ItemData>();
        itemData.name = this.name;
        itemData.description = this.description;
        itemData.attack = this.attack;
        itemData.defense = this.defense;
        itemData.healing = this.healing;
        itemData.sprite = this.sprite;
        return itemData;
    }

}
