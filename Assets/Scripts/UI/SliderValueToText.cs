using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueToText : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateValue()
    {
        if(slider!=null && text!=null)
        {
            int sliderValue = (int)(slider.value);
            text.SetText(sliderValue.ToString());
        }
    }
}
