using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField]
    private Color initialColor;
    [SerializeField] 
    private Color finalColor;
    [SerializeField]
    private Vector3 initialPosition;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Vector3 finalOffset;
    [SerializeField]
    private float fadeDuration;

    private float fadeStartTime;
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        fadeStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float progress = (Time.time - fadeStartTime) / fadeDuration;
        if (progress <= 1)
        {
            transform.position = Vector3.Lerp(initialPosition, initialPosition+finalOffset, progress);
            text.color = Color.Lerp(initialColor, finalColor, progress);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetParentCanvas(GameObject canvas)
    {
        transform.SetParent(canvas.transform);
        initialPosition = Camera.main.WorldToScreenPoint(transform.parent.transform.parent.position+offset);
    }

    public void SetText(string txt)
    {
        if(text!=null) text.SetText(txt);
    }
}
