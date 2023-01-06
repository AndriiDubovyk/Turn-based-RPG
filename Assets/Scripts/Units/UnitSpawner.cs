using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField]
    private UnitList unitList;

    public void SpawnUnit(UnitData unitData, Vector3 position, int health = -1)
    {
        foreach (Unit unit in unitList.units)
        {
            if (unit.unitData.Equals(unitData))
            {
                Vector3 newPos = new Vector3(position.x, position.y, unit.gameObject.transform.position.z);
                unit.gameObject.transform.position = newPos;
                Instantiate(unit.gameObject);
                if (health != -1) unit.SetHealth(health);
                break;
            }
        }
    }

    public void SpawnUnit(string unitName, Vector3 position, int health = -1)
    {
        foreach (Unit unit in unitList.units)
        {
            if (unit.unitData.name.Equals(unitName))
            {
                Vector3 newPos = new Vector3(position.x, position.y, unit.gameObject.transform.position.z);
                unit.gameObject.transform.position = newPos;
                Instantiate(unit.gameObject);
                //if (health != -1) unit.SetHealth(health);
                break;
            }
        }
    }
}
