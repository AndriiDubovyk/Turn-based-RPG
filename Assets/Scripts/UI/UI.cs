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
    private GameObject statsButton;
    [SerializeField]
    private GameObject inventoryPanel;
    [SerializeField]
    private GameObject resultPanel;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private GameObject levelExitPanel;
    [SerializeField]
    private GameObject statsPanel;

    private bool isMouseOverInvetoryButton;
    private bool isMouseOverTakeItemButton;
    private bool isMouseOverMenuButton;
    private bool isMouseOverStatsButton;

    // Start is called before the first frame update
    void Start()
    {
        isMouseOverInvetoryButton = false;
        isMouseOverTakeItemButton = false;
        isMouseOverMenuButton = false;
        isMouseOverStatsButton = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsUIBlockingActions()
    {
        return IsMouseOverUI() || IsInventoryOpened() || IsResultPanelActive() || IsMenuOpened() || IsLevelExitPanelOpened() || IsStatsMenuOpened();
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

    private bool IsStatsMenuOpened()
    {
        return statsPanel != null && statsPanel.activeSelf;
    }

    private bool IsMouseOverUI()
    {
        return isMouseOverInvetoryButton || isMouseOverTakeItemButton || isMouseOverMenuButton || isMouseOverStatsButton; 
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

    public void EnterStatsButton(bool isMouseOver)
    {
        isMouseOverStatsButton = isMouseOver;
    }
}
