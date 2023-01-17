using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField]
    private VolumeSettings musicVolumeSettings;
    [SerializeField]
    private VolumeSettings uiSoundsVolumeSettings;
    [SerializeField]
    private VolumeSettings gameSoundsVolumeSettings;

    private CrossSceneAudioManager audioManager;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("cross_scene_audio");
        if (objs.Length > 0) audioManager = objs[0].GetComponent<CrossSceneAudioManager>();
        musicVolumeSettings.SetValue(PlayerPrefs.GetInt("musicVolume", 50));
        uiSoundsVolumeSettings.SetValue(PlayerPrefs.GetInt("uiVolume", 50));
        gameSoundsVolumeSettings.SetValue(PlayerPrefs.GetInt("gameVolume", 50));
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void ApplySettings()
    {
        PlayerPrefs.SetInt("musicVolume", musicVolumeSettings.GetValue());
        PlayerPrefs.SetInt("uiVolume", uiSoundsVolumeSettings.GetValue());
        PlayerPrefs.SetInt("gameVolume", gameSoundsVolumeSettings.GetValue());
        if (audioManager != null) audioManager.SetValueFromPrefs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
