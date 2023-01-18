using System.Collections;
using System.Collections.Generic;
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
    private Color selectedButtonColor;

    // Start is called before the first frame update
    void Start()
    {
        SelectPanel(alchemistPannel);
        gameObject.SetActive(false);
    }

    public void ShowPanel(GameObject panel)
    {
        ClearSelection();
        SelectPanel(panel);
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
