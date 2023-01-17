using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private GameObject player;
    private ResultPanel resultPanel;

    private bool isExitProcessed = false;
    private GameProcessInfo gpi;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("game_process_info");
        if (objs.Length > 0) gpi = objs[0].GetComponent<GameProcessInfo>();
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
            if(gpi.CurrentDungeonLevel == gpi.MaxDungeonLevel)
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
