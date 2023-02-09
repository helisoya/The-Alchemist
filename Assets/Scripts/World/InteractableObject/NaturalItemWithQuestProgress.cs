using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalItemWithQuestProgress : NaturalItemObject
{
    [SerializeField] private string questToIncrement;

    protected override void InteractionEvent()
    {
        if (GameManager.player.AddItem(GameManager.instance.GetItem(itemName), 1))
        {
            PlayerHotBarUI.instance.RefreshHotBar();

            GameManager.player.IncrementQuest(questToIncrement);
            Destroy(gameObject);
        }

    }
}
