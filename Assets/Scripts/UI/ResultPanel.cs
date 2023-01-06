using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    public void Display()
    {
        gameObject.SetActive(true);
    }

    // true - win, false - lose
    public void SetResult(bool result) 
    {
        if(result)
        {
            text.SetText($"You comlete all {GameProcessInfo.MaxLevel} levels");
        }
        else
        {
            text.SetText($"You die");
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        GameObject.Find("GameHandler").GetComponent<GameSaver>().DeleteSave();
        GameProcessInfo.CurrentLevel = 1;
        SceneManager.LoadScene("RandomLevel");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
