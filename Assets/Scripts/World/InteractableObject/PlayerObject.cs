using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerObject : ToolObject
{
    private int currentSlot;

    protected override bool canCheckForClick {get{return mouseIn && canInteract && currentPlayerItemIsOK;}}



    protected override void InteractionEvent(){
        currentSlot = PlayerHotBarUI.instance.GetCurrentSlot();
        Item item = GameManager.player.GetItemFromSlot(currentSlot);
        if(item == null) return;
        
        if(item.itemAttributes.Count != 0){
            GameManager.instance.AddEffectsToPlayer(item.itemAttributes);
            GameManager.player.DecrementSlot(currentSlot);
            PlayerHotBarUI.instance.RefreshHotBar();
            UIEffectManager.instance.RefreshAllEffects();
        }
    }

}
