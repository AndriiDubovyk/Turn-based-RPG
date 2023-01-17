using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    private CrossSceneAudioManager audioManager;

    [SerializeField]
    private SettingsPanel settingsPanel;

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("cross_scene_audio");
        if (objs.Length > 0) audioManager = objs[0].GetComponent<CrossSceneAudioManager>();
    }

    public void PlayButtonSound()
    {
        if (audioManager != null) audioManager.PlayDefaultUISound();
    }

    public void ToggleSettingPanel()
    {
        if (audioManager != null) audioManager.PlayDefaultUISound();
        settingsPanel.Toggle();
    }

    public void OpenVillageMenu()
    {
        if (audioManager != null) audioManager.PlayDefaultUISound();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
