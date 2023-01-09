using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{

    [SerializeField]
    private GameObject healthLine;
    [SerializeField]
    private TextMeshProUGUI healthText;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        Vector3 scale = healthLine.transform.localScale;
        scale.x = ((float)currentHealth) / maxHealth;
        healthLine.transform.localScale = scale;
        healthText.SetText(currentHealth.ToString());
    }
}
