using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageInfo : MonoBehaviour
{
    public VillageUpgradesData villageUpgradesData;

    private int alchemistLevel;
    private int smithLevel;
    private int librarianLevel;
    private int gold;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("village_info");
        if (objs.Length > 1)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        alchemistLevel = PlayerPrefs.GetInt("alchemistLevel", 0);
        smithLevel = PlayerPrefs.GetInt("smithLevel", 0);
        librarianLevel = PlayerPrefs.GetInt("librarianLevel", 0);
        gold = PlayerPrefs.GetInt("gold", 0);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("alchemistLevel", alchemistLevel);
        PlayerPrefs.SetInt("smithLevel", smithLevel);
        PlayerPrefs.SetInt("librarianLevel", librarianLevel);
        PlayerPrefs.SetInt("gold", gold);
    }

    public int GetAlchemistLevel()
    {
        return alchemistLevel;
    }

    public int GetSmithLevel()
    {
        return smithLevel;
    }


    public int GetLibrarianLevel()
    {
        return librarianLevel;
    }

    public int GetAlchemistUpgradePrice() // -1 if level max alredy
    {
        if(alchemistLevel<villageUpgradesData.alchemistUpgrades.Count)
        {
            return villageUpgradesData.alchemistUpgrades[alchemistLevel].price;
        }
        return -1;
    }

    public int GetSmithUpgradePrice() // -1 if level max alredy
    {
        if (smithLevel < villageUpgradesData.smithUpgrades.Count)
        {
            return villageUpgradesData.smithUpgrades[smithLevel].price;
        }
        return -1;
    }


    public int GetLibrarianUpgradePrice() // -1 if level max alredy
    {
        if (librarianLevel < villageUpgradesData.librarianUpgrades.Count)
        {
            return villageUpgradesData.librarianUpgrades[librarianLevel].price;
        }
        return -1;
    }

    public bool CanBuyAlchemistUpgrade()
    {
        return gold >= GetAlchemistUpgradePrice();
    }


    public bool CanBuySmithUpgrade()
    {
        return gold >= GetSmithUpgradePrice();
    }

    public bool CanBuyLibrarianUpgrade()
    {
        return gold >= GetLibrarianUpgradePrice();
    }

    public int GetRewardByDungeonLevel(int level)
    {
        if(level>0)
        {
            int reward = villageUpgradesData.rewardData.goldForFirstLevelCompleting;
            for (int i = 2; i <= level; i++)
            {
                reward += villageUpgradesData.rewardData.goldForFirstLevelCompleting + villageUpgradesData.rewardData.goldIncreasePerLevel * (i - 1);
            }
            GetReward(reward);
            return reward;
        }
        return 0;
    }


    public void GetReward(int reward)
    {
        gold += reward;
        Save();
    }

    public int GetGold()
    {
        return gold;
    }

    public void BuyAlchemistUpgrade()
    {
        if(CanBuyAlchemistUpgrade())
        {
            gold -= GetAlchemistUpgradePrice();
            alchemistLevel++;
        }
        Save();
    }

    public void BuySmithUpgrade()
    {
        if (CanBuySmithUpgrade())
        {
            gold -= GetSmithUpgradePrice();
            smithLevel++;
        }
        Save();
    }

    public void BuyLibrarianUpgrade()
    {
        if (CanBuyLibrarianUpgrade())
        {
            gold -= GetLibrarianUpgradePrice();
            librarianLevel++;
        }
        Save();
    }

    public string GetAlchemistUpgradeText(int level)
    {
        if (level<=0 || level > villageUpgradesData.alchemistUpgrades.Count) return "";
        string res = "";
        VillageUpgradesData.AlchemistUpgrade upgrade = villageUpgradesData.alchemistUpgrades[level - 1];
        if (upgrade.plusStartPotion > 0)
            res += $"Start with additional {upgrade.plusStartPotion} healing potion\n";
        if (upgrade.plusPotionPower > 0)
            res += $"Healing potions heal {upgrade.plusPotionPower} more health\n";
        return res;
    }

    public string GetSmithUpgradeText(int level)
    {
        if (level <= 0 || level > villageUpgradesData.smithUpgrades.Count) return "";
        string res = "";
        VillageUpgradesData.SmithUpgrade upgrade = villageUpgradesData.smithUpgrades[level - 1];
        if (upgrade.startWeapon != null)
            res += $"Start weapon: '{upgrade.startWeapon.name}'\n";
        if (upgrade.startArmor != null)
            res += $"Start armor: '{upgrade.startArmor.name}'\n";
        if (upgrade.plusWeaponAttack > 0)
            res += $"You will have {upgrade.plusWeaponAttack} more attack\n";
        if (upgrade.plusArmorDefense > 0)
            res += $"You will have {upgrade.plusArmorDefense} more defense\n";
        return res;
    }

    public string GetLibrarianUpgradeText(int level)
    {
        if (level <= 0 || level > villageUpgradesData.librarianUpgrades.Count) return "";
        string res = "";
        VillageUpgradesData.LibrarianUpgrade upgrade = villageUpgradesData.librarianUpgrades[level - 1];
        if (upgrade.againstEnemy != null)
        {
            if (upgrade.attackBonus > 0)
                res += $"Get {upgrade.attackBonus} more attack against {upgrade.againstEnemy.name}s\n";
            if (upgrade.defenseBonus > 0)
                res += $"Get {upgrade.defenseBonus} more defense against {upgrade.againstEnemy.name}s\n";
        }
            
        return res;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
