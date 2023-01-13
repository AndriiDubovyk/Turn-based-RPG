using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitPanel : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    private AudioSource uiSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        uiSound.Play();
        gameObject.SetActive(false);
    }

    public void Confirm()
    {
        uiSound.Play();
        GameProcessInfo.CurrentDungeonLevel++;
        player.GetComponent<PlayerUnit>().SaveData();
        SceneManager.LoadScene("RandomLevel");
    }
}
