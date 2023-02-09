using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestGUIButton : MonoBehaviour
{
    private int index;

    private QuestsGUI gui;

    [SerializeField] private TextMeshProUGUI text;

    public void Init(int i, Quest q, QuestsGUI g)
    {
        index = i;
        gui = g;
        text.text = Locals.GetLocal("Quest_" + q.questID + "_Name");
    }

    public void ButtonPressEvent()
    {
        gui.ShowQuest(index);
    }
}
