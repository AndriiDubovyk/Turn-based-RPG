using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelInfo : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        SetLevelInfo(GameProcessInfo.CurrentLevel);
    }

    public void SetLevelInfo(int level)
    {
        gameObject.GetComponent<TextMeshProUGUI>().SetText($"Level: {level}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
