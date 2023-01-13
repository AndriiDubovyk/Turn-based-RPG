using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsPannel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private TextMeshProUGUI attackText;
    [SerializeField]
    private TextMeshProUGUI defenseText;

    private PlayerUnit player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerUnit>();
        gameObject.SetActive(false);
    }

    public void Toggle()
    { 
        if (player.GetState() == Unit.State.IsThinking) // Possible only on player's turn
        {
            gameObject.SetActive(!gameObject.activeSelf);
            UpdateStats();
        }
    }

    public void UpdateStats()
    {
        int lvl = player.GetLevel();
        int exp = player.GetExp();
        int expToNextLvl = player.levelingData.GetExpToNextLevel(lvl+1);
        levelText.SetText($"Level {lvl}\n({exp}/{expToNextLvl})");
        healthText.SetText(player.GetMaxHP().ToString());
        attackText.SetText(player.GetAttack().ToString());
        defenseText.SetText(player.GetDefense().ToString());
    }
}
