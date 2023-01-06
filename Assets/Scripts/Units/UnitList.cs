using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewUnitList", menuName = "SO/UnitList")]
public class UnitList : ScriptableObject
{
    public List<Unit> units;
}

