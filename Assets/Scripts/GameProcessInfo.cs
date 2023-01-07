using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Save cross scene data
public static class GameProcessInfo
{
    public static int CurrentLevel { get; set; } = 1;
    public static int MaxLevel { get; set; } = 5; //5

    // Player cross scene data
    public static int Attack { get; set; } = 0;
    public static int CurrentHP { get; set; } = 0;
    public static int Defense { get; set; } = 0;
    public static ItemData[] Inventory { get; set; } = new ItemData[6];
    public static ItemData EquipedWeapon { get; set; } = null;
    public static ItemData EquipedArmor { get; set; } = null;
}
    