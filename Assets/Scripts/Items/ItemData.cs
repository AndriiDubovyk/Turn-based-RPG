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
    public int healing;

    public Sprite sprite;

    public ItemData Clone()
    {
        ItemData itemData = new ItemData();
        itemData.name = this.name;
        itemData.description = this.description;
        itemData.attack = this.attack;
        itemData.healing = this.healing;
        itemData.sprite = this.sprite;
        return itemData;
    }

}
