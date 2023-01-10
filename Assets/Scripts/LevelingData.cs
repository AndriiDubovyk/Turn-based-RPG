using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelingData", menuName = "SO/LevelingData")]
public class LevelingData : ScriptableObject
{
    public int expirienceToSecondLevel;
    public int expirienceIncreaseForLevel;

    public int GetExpToNextLevel(int nextLevel)
    {
        if (nextLevel < 3) return expirienceToSecondLevel;
        else return expirienceToSecondLevel + (expirienceIncreaseForLevel*(nextLevel-2));
    }
}
