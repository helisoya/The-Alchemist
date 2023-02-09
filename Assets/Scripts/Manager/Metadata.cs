using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metadata
{
    public int worldMapIconIndex;

    public string internalName;

    public bool outdoor;

    public Metadata()
    {
        outdoor = false;
        worldMapIconIndex = 0;
        internalName = "";
    }
}
