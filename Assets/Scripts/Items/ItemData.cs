using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "SO/ItemData")]
public class ItemData : ScriptableObject
{
    public new string name;
    public string description;
    public int attack;

    public Sprite sprite;
}
