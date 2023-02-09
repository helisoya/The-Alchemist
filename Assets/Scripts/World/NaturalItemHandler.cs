using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalItemHandler : MonoBehaviour
{
    [SerializeField] private GameObject prefabItem;


    void Start()
    {
        foreach (NaturalItem item in GameManager.naturalItems)
        {
            if (item.map.Equals(GameManager.map))
            {
                Instantiate(prefabItem, transform.GetChild(item.spot)).GetComponent<NaturalItemObject>().SetNaturalItem(item);
            }
        }
    }
}
