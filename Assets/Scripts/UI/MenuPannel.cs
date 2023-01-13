using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPannel : MonoBehaviour
{
    private GameSaver gs;
    private PlayerUnit player;

    [SerializeField]
    private AudioSource uiSound;

    // Start is called before the first frame update
    void Start()
    {
        gs = GameObject.Find("GameHandler").GetComponent<GameSaver>();
        player = GameObject.Find("Player").GetComponent<PlayerUnit>();
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        if (player.GetState() == Unit.State.IsThinking) // Possible only on player's turn
        {
            uiSound.Play();
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    public void SaveAndExit()
    {
        uiSound.Play();
        gs.Save();
        SceneManager.LoadScene("Menu");
    }
}
