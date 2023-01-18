using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBlock : MonoBehaviour
{
    [SerializeField]
    private Image background;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI upgradeText;

    [SerializeField]
    private Color notBoughtColor;
    [SerializeField]
    private Color boughtColor;


    public void SetData(int level, string upgrade)
    {
        levelText.SetText($"Level {level}");
        upgradeText.SetText(upgrade);
    }

    public void SetActive(bool isActive)
    {
        if (isActive) background.color = boughtColor;
        else background.color = notBoughtColor;
    }

}
