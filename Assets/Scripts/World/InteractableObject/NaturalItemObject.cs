using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalItemObject : InteractableObject
{
    [SerializeField] protected string itemName;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    private NaturalItem itemRef;

    protected override void InteractionEvent()
    {
        if (GameManager.player.AddItem(GameManager.instance.GetItem(itemName), 1))
        {
            GameManager.naturalItems.Remove(itemRef);
            PlayerHotBarUI.instance.RefreshHotBar();
            Destroy(gameObject);
        }

    }


    public void SetItem(string item)
    {
        itemName = item;
        spriteRenderer.sprite = GameManager.instance.GetItem(itemName).GetItemSprite();
    }

    public void SetNaturalItem(NaturalItem item)
    {
        itemRef = item;
        SetItem(itemRef.itemName);
    }
}
