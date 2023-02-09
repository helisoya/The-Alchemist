using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{

    private int startingKeycode = 49;


    void Update(){
        for(int i = startingKeycode;i<startingKeycode+GameManager.player.maxInHotBar;i++){
            if(Input.GetKeyDown((KeyCode)i)){
                GameManager.player.currentSlot = i-startingKeycode;
                PlayerHotBarUI.instance.RefreshSelection();
                PlayerItemPlacer.instance.enabled = GameManager.player.GetItemFromSlot(GameManager.player.currentSlot) != null && GameManager.player.GetItemFromSlot(GameManager.player.currentSlot).itemType == Item.Type.PLACEABLE;
                RefreshAllToolObjects();
                break;
            }
        }
    }


    void RefreshAllToolObjects(){
        ToolObject[] objs = FindObjectsOfType<ToolObject>();
        foreach(ToolObject obj in objs){
            obj.RefreshObject();
        }
    }
}
