using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.Equals(gameObject.transform.position))
        {
            GameProcessInfo.CurrentLevel++;
            SceneManager.LoadScene("RandomLevel");
        }
    }
}
