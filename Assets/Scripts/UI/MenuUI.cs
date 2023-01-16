using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    private CrossSceneAudioManager audioManager;

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("cross_scene_audio");
        if (objs.Length > 0) audioManager = objs[0].GetComponent<CrossSceneAudioManager>();
    }

    public void Play()
    {
        if (audioManager != null) audioManager.PlayDefaultUISound();
        SceneManager.LoadScene("RandomLevel");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
