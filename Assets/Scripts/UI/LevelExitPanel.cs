using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitPanel : MonoBehaviour
{
    private GameObject player;
    private CrossSceneAudioManager audioManager;
    private GameProcessInfo gpi;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        GameObject[] objs = GameObject.FindGameObjectsWithTag("game_process_info");
        if (objs.Length > 0) gpi = objs[0].GetComponent<GameProcessInfo>();
        objs = GameObject.FindGameObjectsWithTag("cross_scene_audio");
        if (objs.Length > 0) audioManager = objs[0].GetComponent<CrossSceneAudioManager>();
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        if (audioManager != null) audioManager.PlayDefaultUISound();
        gameObject.SetActive(false);
    }

    public void Confirm()
    {
        if (audioManager != null) audioManager.PlayDefaultUISound();
        gpi.CurrentDungeonLevel++;
        player.GetComponent<PlayerUnit>().SaveData();
        GameObject.Find("LoadingScene").GetComponent<LoadingScene>().LoadScene(1);
    }
}
