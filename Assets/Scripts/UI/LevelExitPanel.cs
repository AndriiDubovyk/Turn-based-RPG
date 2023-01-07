using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitPanel : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }

    public void Confirm()
    {
        GameProcessInfo.CurrentLevel++;
        player.GetComponent<PlayerUnit>().SaveData();
        SceneManager.LoadScene("RandomLevel");
    }
}
