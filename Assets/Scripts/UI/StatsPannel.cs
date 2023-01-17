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
    private MainCamera mainCamera;
    private CrossSceneAudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("cross_scene_audio");
        if (objs.Length > 0) audioManager = objs[0].GetComponent<CrossSceneAudioManager>();
        player = GameObject.Find("Player").GetComponent<PlayerUnit>();
        mainCamera = GameObject.Find("MainCamera").GetComponent<MainCamera>();
        gameObject.SetActive(false);
    }

    public void Toggle()
    { 
        if (player.GetState() == Unit.State.IsThinking) // Possible only on player's turn
        {
            if (audioManager != null) audioManager.PlayDefaultUISound();
            gameObject.SetActive(!gameObject.activeSelf);
            if (gameObject.activeSelf) mainCamera.CenterOnPlayer();
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
