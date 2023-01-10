using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLevelInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject expirienceLine;
    [SerializeField]
    private TextMeshProUGUI levelText;

    public void UpdateLevelInfo(int currentLevel, int currentExp, int nextLevelExp)
    {
        Vector3 scale = expirienceLine.transform.localScale;
        scale.x = ((float)currentExp) / nextLevelExp;
        expirienceLine.transform.localScale = scale;
        levelText.SetText(currentLevel.ToString());
    }
}
