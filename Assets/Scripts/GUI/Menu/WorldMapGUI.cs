using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldMapGUI : MenuGUITab
{
    [SerializeField] private Transform iconsParent;
    [SerializeField] private TextMeshProUGUI zoneNameText;
    public override void OnOpen()
    {
        base.OnOpen();
        zoneNameText.text = Locals.GetLocal(GameManager.map+"_Name");

        int selected = GameManager.instance.GetMetadataOfMap(GameManager.map).worldMapIconIndex;
        for(int i = 0;i < iconsParent.childCount;i++){
            iconsParent.GetChild(i).gameObject.SetActive(i==selected);
        }
    }
}
