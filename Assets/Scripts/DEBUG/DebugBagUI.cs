using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBagUI : PlayerBagGUI
{
    [SerializeField] private GameObject realRoot;

    [SerializeField] private Transform allItemsRoot;


    private bool opened;

    private bool inDebugMode;

    public static DebugBagUI instance;


    void Start()
    {
        instance = this;
        opened = false;
        CloseBag();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Open(!opened, true);
        }
    }

    public void Open(bool value, bool debug)
    {
        inDebugMode = debug;
        opened = value;
        realRoot.SetActive(opened);
        if (opened)
        {
            Time.timeScale = 0;
            OpenBag();
        }
        else
        {
            Time.timeScale = 1;
            CloseBag();
        }
    }

    public override void OpenBag()
    {
        base.OpenBag();

        foreach (string key in GameManager.instance.allItemsKeys)
        {
            Instantiate(prefabItemGUI, allItemsRoot).GetComponent<InventoryGUI_ItemSlot>().Init(GameManager.instance.GetItem(key), 50, this);
        }
        allItemsRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(
            allItemsRoot.GetComponent<RectTransform>().sizeDelta.x,
            60 * GameManager.instance.allItemsKeys.Count / 8);
    }

    public override void CloseBag()
    {
        base.CloseBag();

        foreach (Transform child in allItemsRoot)
        {
            Destroy(child.gameObject);
        }
    }

}
