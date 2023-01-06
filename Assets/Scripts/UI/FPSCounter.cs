using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    private int avgFrameRate;
    private TextMeshProUGUI display_Text;

    private void Start()
    {
        display_Text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        float current = 0;
        current = Time.frameCount / Time.time;
        avgFrameRate = (int)current;
        display_Text.SetText($"FPS: {avgFrameRate.ToString() }");
    }
}
