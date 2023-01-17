using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{

    [SerializeField]
    private GameObject inventoryButton;
    [SerializeField]
    private GameObject skipTurnButton;
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
    [SerializeField]
    private GameObject itemTakeScrollView;

    private PlayerUnit player;
    private GridManager gm;
    private CrossSceneAudioManager audioManager;

    private bool 
        isMouseOverInvetoryButton,
        isMouseOverSkipTurnButton,
        isMouseOverMenuButton,
        isMouseOverStatsButton,
        isMouseOverItemTakeScroll;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("cross_scene_audio");
        if (objs.Length > 0) audioManager = objs[0].GetComponent<CrossSceneAudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerUnit>();
        gm = GameObject.Find("Grid").GetComponent<GridManager>();
        isMouseOverInvetoryButton = false;
        isMouseOverSkipTurnButton = false;
        isMouseOverMenuButton = false;
        isMouseOverStatsButton = false;
        isMouseOverItemTakeScroll = false;
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

    public bool IsMouseOverUI()
    {
        return isMouseOverInvetoryButton || isMouseOverMenuButton || isMouseOverStatsButton || isMouseOverSkipTurnButton || isMouseOverItemTakeScroll; 
    } 

    public void EnterInventoryButton(bool isMouseOver)
    {
        isMouseOverInvetoryButton = isMouseOver;
    }

    public void EnterItemTakeScroll(bool isMouseOver)
    {
        isMouseOverItemTakeScroll = isMouseOver;
    }

    public void EnterSkipTurnButton(bool isMouseOver)
    {
        isMouseOverSkipTurnButton = isMouseOver;
    }

    public void EnterMenuButton(bool isMouseOver)
    {
        isMouseOverMenuButton = isMouseOver;
    }

    public void EnterStatsButton(bool isMouseOver)
    {
        isMouseOverStatsButton = isMouseOver;
    }

    public void SkipTurn()
    {
        if (player.GetState()==Unit.State.IsThinking)
        {
            player.SkipTurn();
            player.ShowFloatingWaitText();
            if (audioManager != null) audioManager.PlayDefaultUISound();
        }
    }

    public void UpdateItemTakeScrollItems()
    {
        itemTakeScrollView.GetComponent<ItemTakeScroll>().UpdateItems(gm.GetItemPickupList());
    }
}
