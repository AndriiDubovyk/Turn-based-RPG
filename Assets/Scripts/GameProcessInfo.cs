using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// Save cross scene data
public class GameProcessInfo : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("game_process_info");
        if (objs.Length > 1)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Destroy(gameObject);
        }
    }


    public int CurrentDungeonLevel { get; set; } = 1;
    public int MaxDungeonLevel { get; set; } = 5; //5

    // Player cross scene data
    public int Attack { get; set; } = 0;
    public int MaxHP { get; set; } = 0;
    public int CurrentHP { get; set; } = 0;
    public int Defense { get; set; } = 0;
    public int Level { get; set; } = 0;
    public int Exp { get; set; } = 0;
    public ItemData[] Inventory { get; set; } = new ItemData[6];
    public ItemData EquipedWeapon { get; set; } = null;
    public ItemData EquipedArmor { get; set; } = null;
}
    