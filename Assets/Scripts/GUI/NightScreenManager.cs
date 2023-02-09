using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightScreenManager : MonoBehaviour
{
    [SerializeField] private Transform parentSeasonIcons;
    void Start()
    {
        parentSeasonIcons.GetChild(GameManager.currentMonth).gameObject.SetActive(true);
    }
}
