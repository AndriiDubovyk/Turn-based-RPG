using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    private CrossSceneAudioManager audioManager;
    private GameProcessInfo gpi;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("game_process_info");
        if (objs.Length > 0) gpi = objs[0].GetComponent<GameProcessInfo>();
        objs = GameObject.FindGameObjectsWithTag("cross_scene_audio");
        if (objs.Length > 0) audioManager = objs[0].GetComponent<CrossSceneAudioManager>();
        Hide();
    }

    public void Display()
    {
        gameObject.SetActive(true);
    }

    // true - win, false - lose
    public void SetResult(bool result) 
    {
        if(result)
        {
            text.SetText($"You comlete all {gpi.MaxDungeonLevel} levels");
        }
        else
        {
            text.SetText($"You die");
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        if (audioManager != null) audioManager.PlayDefaultUISound();
        GameObject.Find("GameHandler").GetComponent<GameSaver>().DeleteSave();
        gpi.CurrentDungeonLevel = 1;
        GameObject.Find("LoadingScene").GetComponent<LoadingScene>().LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
