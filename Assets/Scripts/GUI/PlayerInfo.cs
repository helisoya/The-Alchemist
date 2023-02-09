using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTime;
    [SerializeField] private RectTransform compass;
    [SerializeField] private DayLabelLocalized textDay;
    [SerializeField] private GoldLocalizedText textGold;

    public static PlayerInfo instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        RefreshDay();
        RefreshGold();
    }

    public void RefreshTime()
    {
        textTime.text = GameManager.instance.inGameHour.ToString() + "H";
        compass.eulerAngles = new Vector3(0, 0, (360f / 24f) * GameManager.instance.inGameHour);
    }

    public void RefreshDay()
    {
        textDay.ReloadText();
    }

    public void RefreshGold()
    {
        textGold.ReloadText();
    }
}
