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
}
