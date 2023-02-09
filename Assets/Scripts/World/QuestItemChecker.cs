using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItemChecker : MonoBehaviour
{
    [SerializeField] private string questName;
    [SerializeField] private int questProgress;

    void Start()
    {
        Quest q = GameManager.player.GetQuest(questName);
        if (q.currentPhase != questProgress)
        {
            Destroy(gameObject);
        }
    }
}
