using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolObject : InteractableObject
{
    [SerializeField] private Item.Type typeRequired;

    protected override bool canCheckForClick { get { return playerInZone && mouseIn && canInteract && currentPlayerItemIsOK; } }

    protected bool currentPlayerItemIsOK
    {
        get
        {
            Item i = GameManager.player.GetItemFromSlot(GameManager.player.currentSlot);
            return i != null && i.itemType == typeRequired;
        }
    }

    public void RefreshObject()
    {
        RefreshCursor();
    }
}
