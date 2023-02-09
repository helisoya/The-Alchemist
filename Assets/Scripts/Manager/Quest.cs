using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questID;

    public int goldReward;
    public int currentPhase;


    public string[] craftRequirement;

    public int maxPhases;


    public Quest(string id)
    {
        questID = id;
        currentPhase = -1;
        maxPhases = 1;
        craftRequirement = new string[1];
    }


    public string GetCraftRequirementForCurrentPhase()
    {
        if (craftRequirement[currentPhase] == null) return "";
        return craftRequirement[currentPhase];
    }
}
