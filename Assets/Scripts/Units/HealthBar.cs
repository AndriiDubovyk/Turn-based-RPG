using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        slider.gameObject.SetActive(currentHealth < maxHealth && currentHealth>0);
        slider.value = currentHealth;
        slider.maxValue = maxHealth;
    }
}
