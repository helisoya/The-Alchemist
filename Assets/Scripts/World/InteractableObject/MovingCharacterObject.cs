using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCharacterObject : InteractableObject
{

    private MovingCharacter model;

    public void Init(MovingCharacter m)
    {
        model = m;
    }

    protected override void InteractionEvent()
    {
        if (!DialogSystem.instance.inDialog)
        {

            if (model.questsAssociated.Count == 0)
            {
                DialogSystem.instance.StartDialog(model.charName + "_Default");
                return;
            }

            foreach (string quest in model.questsAssociated)
            {
                Quest q = GameManager.player.GetQuest(quest);

                if (q.currentPhase == q.maxPhases)
                {
                    continue;
                }

                if (q.currentPhase == -1)
                {
                    print(model.charName + "_" + quest + "_Ask");
                    DialogSystem.instance.StartDialog(model.charName + "_" + quest + "_Ask");
                    return;
                }
                else if (q.currentPhase == q.maxPhases - 1 && GameManager.player.RemoveItem(q.craftRequirement[q.maxPhases - 2]))
                {
                    print(model.charName + "_" + quest + "_End");
                    PlayerHotBarUI.instance.RefreshHotBar();
                    DialogSystem.instance.StartDialog(model.charName + "_" + quest + "_End");
                    return;
                }

                print(model.charName + "_Default");
                DialogSystem.instance.StartDialog(model.charName + "_Default");
                return;
            }
        }
    }
}
