using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneAudioManager : MonoBehaviour
{
    // music
    [SerializeField]
    private AudioSource mainTheme;

    // ui
    [SerializeField]
    private AudioSource defaultUISound;

    // game
    [SerializeField]
    private AudioSource bagSound;
    [SerializeField]
    private AudioSource dropItemSound;
    [SerializeField]
    private AudioSource attackSound;
    [SerializeField]
    private AudioSource itemEquipSound;
    [SerializeField]
    private AudioSource potionSound;
    [SerializeField]
    private AudioSource takeItemSound;
    [SerializeField]
    private AudioSource walkSound;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("cross_scene_audio");
        if (objs.Length > 1)
            Destroy(gameObject);

        SetValueFromPrefs();

        DontDestroyOnLoad(gameObject);
    }

    public void SetValueFromPrefs()
    {
        SetMusicVolume(PlayerPrefs.GetInt("musicVolume", 50));
        SetUIVolume(PlayerPrefs.GetInt("uiVolume", 50));
        SetGameVolume(PlayerPrefs.GetInt("gameVolume", 50));
    }

    public void PlayDefaultUISound()
    {
        defaultUISound.Play();
    }

    public void PlayBagSound()
    {
        bagSound.Play();
    }

    public void PlayDropItemSound()
    {
        dropItemSound.Play();
    }

    public void PlayAttackSound()
    {
        attackSound.Play();
    }

    public void PlayItemEquipSound()
    {
        itemEquipSound.Play();
    }

    public void PlayPotionSound()
    {
        potionSound.Play();
    }

    public void PlayTakeItemSound()
    {
        takeItemSound.Play();
    }

    public void PlayWalkSound()
    {
        walkSound.Play();
    }

    public void SetMusicVolume(int volume)
    {
        mainTheme.volume = volume * 0.01f;
    }

    public void SetUIVolume(int volume)
    {
        defaultUISound.volume = volume * 0.01f;
    }

    public void SetGameVolume(int volume)
    {
        bagSound.volume = volume * 0.01f;
        dropItemSound.volume = volume * 0.01f;
        attackSound.volume = volume * 0.01f; 
        itemEquipSound.volume = volume * 0.01f; 
        potionSound.volume = volume * 0.01f;        
        takeItemSound.volume = volume * 0.01f;     
        walkSound.volume = volume * 0.01f;
    }

}
