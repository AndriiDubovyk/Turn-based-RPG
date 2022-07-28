using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : MonoBehaviour
{
    public int maxHP;
    public int attack;
    private int currentHP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(CombatUnit another)
    {
        Debug.Log(tag + " attack " + another.tag + " with " + attack + " dmg");
        another.TakeDamage(this.attack);
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        if (currentHP < 0) currentHP = 0;
        Debug.Log(tag + " was attacked with " + damageAmount + " dmg. Current HP: " + currentHP);
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }
}
