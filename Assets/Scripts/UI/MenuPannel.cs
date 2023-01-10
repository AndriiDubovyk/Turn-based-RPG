using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPannel : MonoBehaviour
{
    private GameSaver gs;

    // Start is called before the first frame update
    void Start()
    {
        gs = GameObject.Find("GameHandler").GetComponent<GameSaver>();
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SaveAndExit()
    {
        gs.Save();
        SceneManager.LoadScene("Menu");
    }
}
