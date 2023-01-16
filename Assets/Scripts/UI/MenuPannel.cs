using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPannel : MonoBehaviour
{
    private GameSaver gs;
    private PlayerUnit player;

    private CrossSceneAudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        gs = GameObject.Find("GameHandler").GetComponent<GameSaver>();
        player = GameObject.Find("Player").GetComponent<PlayerUnit>();
        gameObject.SetActive(false);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("cross_scene_audio");
        if (objs.Length > 0) audioManager = objs[0].GetComponent<CrossSceneAudioManager>();
    }

    public void Toggle()
    {
        if (player.GetState() == Unit.State.IsThinking) // Possible only on player's turn
        {
            if (audioManager != null) audioManager.PlayDefaultUISound();
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    public void SaveAndExit()
    {
        if (audioManager != null) audioManager.PlayDefaultUISound();
        gs.Save();
        SceneManager.LoadScene("Menu");
    }
}
