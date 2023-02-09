using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestsGUI : MenuGUITab
{
    [SerializeField] private GameObject prefabButton;
    [SerializeField] private Transform prefabParent;
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questReward;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private TextMeshProUGUI questCurrent;

    private List<Quest> currentQuests;


    public override void OnOpen()
    {
        base.OnOpen();

        foreach (Transform child in prefabParent)
        {
            Destroy(child.gameObject);
        }


        currentQuests = new List<Quest>();

        foreach (Quest q in GameManager.player.GetAllQuests())
        {
            if (!(q.currentPhase >= 0 && q.currentPhase < q.maxPhases)) continue;

            currentQuests.Add(q);
            Instantiate(prefabButton, prefabParent).GetComponent<QuestGUIButton>().Init(currentQuests.Count - 1, q, this);
        }

        ShowQuest(0);
    }

    public void ShowQuest(int index)
    {
        if (currentQuests.Count <= index)
        {
            questName.text = "";
            questCurrent.text = "";
            questDescription.text = "";
            questReward.text = "";
            return;
        }


        Quest q = currentQuests[index];
        questName.text = Locals.GetLocal("Quest_" + q.questID + "_Name");
        questDescription.text = Locals.GetLocal("Quest_" + q.questID + "_Description");
        questCurrent.text = Locals.GetLocal("Quest_" + q.questID + "_" + q.currentPhase);
        questReward.text = q.goldReward + " " + Locals.GetLocal("Game_Currency");
    }


}
