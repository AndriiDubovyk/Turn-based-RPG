using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelInfo : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().SetText($"Level: {GameProcessInfo.CurrentLevel}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
