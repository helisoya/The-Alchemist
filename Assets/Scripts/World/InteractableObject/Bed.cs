using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : InteractableObject
{
    protected override void InteractionEvent()
    {
        GameCursor.ChangeCursor("defaultCursor");
        GameManager.instance.InitializeNewDay(true);
    }
}
