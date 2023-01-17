using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonDepthInfo : MonoBehaviour
{

    private GameProcessInfo gpi;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("game_process_info");
        if (objs.Length > 0) gpi = objs[0].GetComponent<GameProcessInfo>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetLevelInfo(gpi.CurrentDungeonLevel);
    }

    public void SetLevelInfo(int level)
    {
        gameObject.GetComponent<TextMeshProUGUI>().SetText($"{level}");
    }

}
