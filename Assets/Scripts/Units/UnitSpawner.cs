using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField]
    private UnitList unitList;

    public GameObject SpawnUnit(UnitData unitData, Vector3 position, int health = -1)
    {
        GameObject spawnedUnit = null;
        foreach (Unit unit in unitList.units)
        {
            if (unit.unitData.Equals(unitData))
            {
                Vector3 newPos = new Vector3(position.x, position.y, unit.gameObject.transform.position.z);
                unit.gameObject.transform.position = newPos;
                spawnedUnit = Instantiate(unit.gameObject);
                if (health != -1) unit.SetHealth(health);
                break;
            }
        }
        return spawnedUnit;
    }

    public GameObject SpawnUnit(string unitName, Vector3 position, int health = -1)
    {
        GameObject spawnedUnit = null;
        foreach (Unit unit in unitList.units)
        {
            if (unit.unitData.name.Equals(unitName))
            {
                Vector3 newPos = new Vector3(position.x, position.y, unit.gameObject.transform.position.z);
                unit.gameObject.transform.position = newPos;
                spawnedUnit = Instantiate(unit.gameObject);
                if (health != -1) spawnedUnit.GetComponent<Unit>().SetHealth(health);
                break;
            }
        }
        return spawnedUnit;
    }
}
