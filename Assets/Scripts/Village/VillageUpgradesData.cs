using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewVillageUpgradesDataa", menuName = "SO/VillageUpgradesData")]
public class VillageUpgradesData : ScriptableObject
{
    [System.Serializable]
    public class AlchemistUpgrade
    {
        public int plusStartPotion;
        public int plusPotionPower;
        public int price;
    }
    public List<AlchemistUpgrade> alchemistUpgrades;

    [System.Serializable]
    public class SmithUpgrade
    {
        public ItemData startWeapon;
        public ItemData startArmor;
        public int plusWeaponAttack;
        public int plusArmorDefense;
        public int price;
    }
    public List<SmithUpgrade> smithUpgrades;


    [System.Serializable]
    public class LibrarianUpgrade
    {
        public UnitData againstEnemy;
        public int attackBonus;
        public int defenseBonus;
        public int price;
    }
    public List<LibrarianUpgrade> librarianUpgrades;

    [System.Serializable]
    public class RewardData
    {
        public int goldForFirstLevelCompleting;
        public int goldIncreasePerLevel;
    }
    public RewardData rewardData;
}
