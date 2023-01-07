using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private GameObject player;
    private ResultPanel resultPanel;

    private bool isExitProcessed = false;

    private void Awake()
    {
        resultPanel = GameObject.Find("ResultPanel").GetComponent<ResultPanel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");  
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.Equals(gameObject.transform.position) && !isExitProcessed)
        {
            isExitProcessed = true;
            GameObject.Find("GameHandler").GetComponent<GameSaver>().DeleteSave();
            if(GameProcessInfo.CurrentLevel == GameProcessInfo.MaxLevel)
            {
                resultPanel.SetResult(true);
                resultPanel.Display();
            }
            else
            {
                GameObject.Find("UICanvas").GetComponent<UI>().ShowLevelExitPanel();
            }
        } else if(!player.transform.position.Equals(gameObject.transform.position) && isExitProcessed)
        {
            isExitProcessed = false;
        }
    }
}
