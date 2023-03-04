using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private TextMeshProUGUI rewardText;
    private CrossSceneAudioManager audioManager;
    private GameProcessInfo gpi;
    private VillageInfo villageInfo;


    private bool isResultSetted = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("game_process_info");
        if (objs.Length > 0) gpi = objs[0].GetComponent<GameProcessInfo>();
        objs = GameObject.FindGameObjectsWithTag("village_info");
        if (objs.Length > 0) villageInfo = objs[0].GetComponent<VillageInfo>();
        objs = GameObject.FindGameObjectsWithTag("cross_scene_audio");
        if (objs.Length > 0) audioManager = objs[0].GetComponent<CrossSceneAudioManager>();
        Hide();
    }

    public void Display()
    {
        gameObject.SetActive(true);
    }

    // Set game result: true - win, false - lose
    public void SetResult(bool result) 
    {
        if (isResultSetted) return;
        isResultSetted = true;
        int reward = 0;
        if(result)
        {
            text.SetText($"You comlete all {gpi.MaxDungeonLevel} levels");
            reward = villageInfo.GetRewardByDungeonLevel(gpi.MaxDungeonLevel);
        }
        else
        {
            text.SetText($"You die");
            reward = villageInfo.GetRewardByDungeonLevel(gpi.CurrentDungeonLevel - 1);
        }
        rewardText.SetText($"+{reward}");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // Restart game
    public void Restart()
    {
        if (audioManager != null) audioManager.PlayDefaultUISound();
        gpi.CurrentDungeonLevel = 1;
        // Delete save for starting new game
        GameObject.Find("GameHandler").GetComponent<GameSaver>().DeleteSave();

        GameObject.Find("LoadingScene").GetComponent<LoadingScene>().LoadScene(1);
    }

    // Back to main menu
    public void BackToMenu()
    {
        if (audioManager != null) audioManager.PlayDefaultUISound();

        // Delete save for starting new game
        GameObject.Find("GameHandler").GetComponent<GameSaver>().DeleteSave();

        GameObject.Find("LoadingScene").GetComponent<LoadingScene>().LoadScene(0);
    }
}
