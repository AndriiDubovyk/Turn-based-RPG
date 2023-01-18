using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private SliderValueToText valueText;

    public int GetValue()
    {
        return (int)slider.value;
    }

    public void SetValue(int newValue)
    {
        slider.value = newValue;
        valueText.UpdateValue();
    }
}
