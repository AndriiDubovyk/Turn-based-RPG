using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPannel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SaveAndExit()
    {
        Application.Quit();
    }
}
