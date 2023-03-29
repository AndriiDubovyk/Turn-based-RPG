using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    private CrossSceneAudioManager audioManager;

    [SerializeField]
    private SettingsPanel settingsPanel;
    [SerializeField]
    private VillagePannel villagePanel;
    [SerializeField]
    private TutorialPanel tutorialPanel;

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

    public void ToggleTutorialPanel()
    {
        if (audioManager != null) audioManager.PlayDefaultUISound();
        tutorialPanel.Toggle();
    }


    public void ToggleVillagePanel()
    {
        if (audioManager != null) audioManager.PlayDefaultUISound();
        villagePanel.Toggle();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
