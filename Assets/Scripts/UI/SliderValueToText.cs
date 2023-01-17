using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueToText : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI text;

    public void UpdateValue()
    {
        int sliderValue = (int)(slider.value);
        text.SetText(sliderValue.ToString());
    }
}
