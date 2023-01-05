using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{

    [SerializeField]
    private GameObject inventoryButton;
    [SerializeField]
    private GameObject takeItemButton;
    [SerializeField]
    private GameObject inventoryPanel;
    [SerializeField]
    private GameObject resultPanel;

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

    public bool IsUIBlockingActions()
    {
        return IsMouseOverUI() || IsInventoryOpened() || IsResultPanelActive();
    }


    private bool IsInventoryOpened()
    {
        return inventoryPanel != null && inventoryPanel.activeSelf;
    }

    private bool IsResultPanelActive()
    {
        return resultPanel.activeSelf;
    }

    private bool IsMouseOverUI()
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
