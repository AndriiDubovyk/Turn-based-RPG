using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonDepthInfo : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        SetLevelInfo(GameProcessInfo.CurrentDungeonLevel);
    }

    public void SetLevelInfo(int level)
    {
        gameObject.GetComponent<TextMeshProUGUI>().SetText($"Dungeon Depth: {level}");
    }

}
