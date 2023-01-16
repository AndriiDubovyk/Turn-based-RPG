using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource defaultUISound;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("cross_scene_audio");
        if (objs.Length > 1)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void PlayDefaultUISound()
    {
        defaultUISound.Play();
    }

}
