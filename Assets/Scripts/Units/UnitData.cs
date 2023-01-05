using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitData", menuName = "SO/UnitData")]
public class UnitData : ScriptableObject
{
    public int maxHP;
    public int attack;
    public int maxAttackDistance;
    public int pathfindingXMaxDistance;
    public int pathfindingYMaxDistance;
    public float moveSpeed;
    public Sprite sprite;

    [System.Serializable]
    public class Drop
    {
        public ItemData itemData;
        public int dropChance;
    }

    public List<Drop> drops;
}
