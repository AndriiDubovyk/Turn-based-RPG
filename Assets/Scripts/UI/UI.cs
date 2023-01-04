using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{

    [SerializeField]
    private GameObject inventoryButton;
    [SerializeField]
    private GameObject takeItemButton;

    private bool isMouseOverInvetoryButton;
    private bool isMouseOverTakeItemButton;

    // Start is called before the first frame update
    void Start()
    {
        isMouseOverInvetoryButton = false;
        isMouseOverTakeItemButton = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsMouseOverUI()
    {
        return isMouseOverInvetoryButton || isMouseOverTakeItemButton; 
    } 

    public void EnterInventoryButton(bool isMouseOver)
    {
        isMouseOverInvetoryButton = isMouseOver;
    }

    public void EnterTakeItemButton(bool isMouseOver)
    {
        isMouseOverTakeItemButton = isMouseOver;
    }
}
