using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VillagePannel : MonoBehaviour
{

    [SerializeField]
    private GameObject alchemistPannel;
    [SerializeField]
    private GameObject smithPannel;
    [SerializeField]
    private GameObject librarianPannel;

    [SerializeField]
    private Button alchemistButton;
    [SerializeField]
    private Button smithButton;
    [SerializeField]
    private Button librarianButton;

    [SerializeField]
    private TextMeshProUGUI goldText;

    [SerializeField]
    private Color selectedButtonColor;

    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private GameObject upgradeBlockPrefab;

    private List<UpgradeBlock> alchemistUpgradeBlocks = new List<UpgradeBlock>();
    private List<UpgradeBlock> smithUpgradeBlocks = new List<UpgradeBlock>();
    private List<UpgradeBlock> librarianUpgradeBlocks = new List<UpgradeBlock>();

    private VillageInfo villageInfo;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("village_info");
        if (objs.Length > 0) villageInfo = objs[0].GetComponent<VillageInfo>();
        InitUpgradeBlocks();
        SelectPanel(alchemistPannel);
        gameObject.SetActive(false);
        UpdateData();
    }

    private void InitUpgradeBlocks()
    {
        float height = ((RectTransform)upgradeBlockPrefab.transform).rect.height;

        // Alchemist
        List<VillageUpgradesData.AlchemistUpgrade> alchemistUpgrades = villageInfo.villageUpgradesData.alchemistUpgrades;
        for (int i = 0; i < alchemistUpgrades.Count; i++)
        {
            GameObject go = Instantiate(upgradeBlockPrefab, alchemistPannel.transform);
            UpgradeBlock ub = go.GetComponent<UpgradeBlock>();
            ub.SetData(i + 1, villageInfo.GetAlchemistUpgradeText(i + 1));
            alchemistUpgradeBlocks.Add(ub);    
        }

        // Smith
        List<VillageUpgradesData.SmithUpgrade> smithUpgrades = villageInfo.villageUpgradesData.smithUpgrades;
        for (int i = 0; i < smithUpgrades.Count; i++)
        {
            GameObject go = Instantiate(upgradeBlockPrefab, smithPannel.transform);
            UpgradeBlock ub = go.GetComponent<UpgradeBlock>();
            ub.SetData(i + 1, villageInfo.GetSmithUpgradeText(i + 1));
            smithUpgradeBlocks.Add(ub);
        }

        // Librarian
        List<VillageUpgradesData.LibrarianUpgrade> librarianUpgrades = villageInfo.villageUpgradesData.librarianUpgrades;
        for (int i = 0; i < librarianUpgrades.Count; i++)
        {
            GameObject go = Instantiate(upgradeBlockPrefab, librarianPannel.transform);
            UpgradeBlock ub = go.GetComponent<UpgradeBlock>();
            ub.SetData(i + 1, villageInfo.GetLibrarianUpgradeText(i + 1));
            librarianUpgradeBlocks.Add(ub);
        }
    }

    public void ShowPanel(GameObject panel)
    {
        ClearSelection();
        SelectPanel(panel);
    }

    private void UpdateData()
    {
        goldText.SetText(villageInfo.GetGold().ToString());
        UpdateBuyButton();
        UpdateBoughtUpgrades();
    }

    private void UpdateBoughtUpgrades()
    {
        for(int i = 1; i<=villageInfo.villageUpgradesData.alchemistUpgrades.Count; i++)
        {
            alchemistUpgradeBlocks[i-1].SetActive(i<=villageInfo.GetAlchemistLevel());
        }
        for (int i = 1; i <= villageInfo.villageUpgradesData.smithUpgrades.Count; i++)
        {
            smithUpgradeBlocks[i-1].SetActive(i <= villageInfo.GetSmithLevel());
        }
        for (int i = 1; i <= villageInfo.villageUpgradesData.librarianUpgrades.Count; i++)
        {
            librarianUpgradeBlocks[i-1].SetActive(i <= villageInfo.GetLibrarianLevel());
        }
    }

    private void SelectPanel(GameObject panel)
    {
        panel.SetActive(true);
        if(panel==alchemistPannel)
        {
            alchemistButton.image.color = selectedButtonColor;
        } else if (panel == smithPannel)
        {
            smithButton.image.color = selectedButtonColor;
        }
        if (panel == librarianPannel)
        {
            librarianButton.image.color = selectedButtonColor;
        }
        UpdateData();
    }


    private void UpdateBuyButton()
    {
        int buyPrice = -1;
        if(alchemistPannel.activeSelf) buyPrice = villageInfo.GetAlchemistUpgradePrice();
        else if (smithPannel.activeSelf) buyPrice = villageInfo.GetSmithUpgradePrice();
        else if (librarianPannel.activeSelf) buyPrice = villageInfo.GetLibrarianUpgradePrice();

        priceText.SetText(buyPrice.ToString());
        if(buyPrice==-1)
        {
            buyButton.gameObject.GetComponent<Image>().color = Color.grey;
            priceText.SetText("");
        }
        else if (villageInfo.GetGold() >= buyPrice) buyButton.gameObject.GetComponent<Image>().color = Color.green;
        else buyButton.gameObject.GetComponent<Image>().color = Color.red;
    }

    public void BuyUpgrade()
    {
        if (alchemistPannel.activeSelf) villageInfo.BuyAlchemistUpgrade();
        else if (smithPannel.activeSelf) villageInfo.BuySmithUpgrade();
        else if (librarianPannel.activeSelf) villageInfo.BuyLibrarianUpgrade();
        UpdateData();
        villageInfo.Save();
    }

    private void ClearSelection()
    {
        librarianPannel.SetActive(false);
        alchemistPannel.SetActive(false);
        smithPannel.SetActive(false);

        librarianButton.image.color = Color.white;
        alchemistButton.image.color = Color.white;
        smithButton.image.color = Color.white;
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
