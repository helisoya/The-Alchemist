using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingBin : ToolObject
{
    protected override bool canCheckForClick { get { return playerInZone && mouseIn && canInteract && hasItemInHand; } }

    protected bool hasItemInHand
    {
        get { return GameManager.player.GetItemFromSlot(GameManager.player.currentSlot) != null; }
    }


    protected override void InteractionEvent()
    {
        int currentSlot = PlayerHotBarUI.instance.GetCurrentSlot();
        Item item = GameManager.player.GetItemFromSlot(currentSlot);
        if (item == null) return;

        GameManager.player.gold += item.sellPrice;
        GameManager.player.DecrementSlot(currentSlot);
        PlayerHotBarUI.instance.RefreshHotBar();
        PlayerInfo.instance.RefreshGold();
    }
}
