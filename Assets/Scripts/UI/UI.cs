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
    private GameObject menuButton;
    [SerializeField]
    private GameObject inventoryPanel;
    [SerializeField]
    private GameObject resultPanel;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private GameObject levelExitPanel;

    private bool isMouseOverInvetoryButton;
    private bool isMouseOverTakeItemButton;
    private bool isMouseOverMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        isMouseOverInvetoryButton = false;
        isMouseOverTakeItemButton = false;
        isMouseOverMenuButton = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsUIBlockingActions()
    {
        return IsMouseOverUI() || IsInventoryOpened() || IsResultPanelActive() || IsMenuOpened() || IsLevelExitPanelOpened();
    }

    public void ShowLevelExitPanel() {
        levelExitPanel.SetActive(true);
    }


    private bool IsInventoryOpened()
    {
        return inventoryPanel != null && inventoryPanel.activeSelf;
    }

    private bool IsLevelExitPanelOpened()
    {
        return levelExitPanel != null && levelExitPanel.activeSelf;
    }

    private bool IsResultPanelActive()
    {
        return resultPanel != null && resultPanel.activeSelf;
    }
    private bool IsMenuOpened()
    {
        return menuPanel!= null && menuPanel.activeSelf;
    }

    private bool IsMouseOverUI()
    {
        return isMouseOverInvetoryButton || isMouseOverTakeItemButton || isMouseOverMenuButton; 
    } 

    public void EnterInventoryButton(bool isMouseOver)
    {
        isMouseOverInvetoryButton = isMouseOver;
    }

    public void EnterTakeItemButton(bool isMouseOver)
    {
        isMouseOverTakeItemButton = isMouseOver;
    }

    public void EnterMenuButton(bool isMouseOver)
    {
        isMouseOverMenuButton = isMouseOver;
    }
}
