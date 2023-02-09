using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingBagUI : PlayerBagGUI
{

    [SerializeField] private CraftingSlot slot1;
    [SerializeField] private CraftingSlot slot2;

    [SerializeField] private CraftingSlot resultSlot;

    [SerializeField] private CraftingGfx[] machinesGfx;

    private Recipe.Machines currentMachine;

    public void OpenBag(Recipe.Machines machine)
    {
        base.OpenBag();
        slot1.SetSlot(-1);
        slot2.SetSlot(-1);
        resultSlot.SetSlot(-1);
        currentMachine = machine;
        for (int i = 0; i < machinesGfx.Length; i++)
        {
            if (i == ((int)machine) - 1)
            {
                machinesGfx[i].ShowMachine();
            }
            else
            {
                machinesGfx[i].HideMachine();
            }
        }
    }

    public void DropAll()
    {

        if (slot1.GetItem() != null && slot1.GetNbItems() != 0)
        {
            InWorldItem item = Instantiate(GameManager.prefabInWorldItem, Player.body.transform.position, new Quaternion()).GetComponent<InWorldItem>();
            item.Init(Player.body.GetComponentInChildren<Collider2D>(), slot1.GetItem(), slot1.GetNbItems());
        }
        if (slot2.GetItem() != null && slot2.GetNbItems() != 0)
        {
            InWorldItem item = Instantiate(GameManager.prefabInWorldItem, Player.body.transform.position, new Quaternion()).GetComponent<InWorldItem>();
            item.Init(Player.body.GetComponentInChildren<Collider2D>(), slot2.GetItem(), slot2.GetNbItems());
        }

        if (resultSlot.GetItem() != null && resultSlot.GetNbItems() != 0)
        {
            InWorldItem item = Instantiate(GameManager.prefabInWorldItem, Player.body.transform.position, new Quaternion()).GetComponent<InWorldItem>();
            item.Init(Player.body.GetComponentInChildren<Collider2D>(), resultSlot.GetItem(), resultSlot.GetNbItems());
        }
    }

    public bool Item1CorrespondsTo(Item item, int number)
    {
        if (!slot1.IsItemSameAs(item)) return false;
        if (item == null) return true;
        return number == slot1.GetNbItems();
    }

    public bool Item2CorrespondsTo(Item item, int number)
    {
        if (!slot2.IsItemSameAs(item)) return false;
        if (item == null) return true;
        return number == slot2.GetNbItems();
    }


    public void TakeItemFromCraftingSlot(CraftingSlot slot)
    {
        bool takeAll = Input.GetKey(KeyCode.LeftShift);


        if (itemMoving == null)
        {
            itemMoving = slot.GetItem();
            if (itemMoving != null)
            {
                int maxLoop = slot.GetNbItems();
                for (int i = 0; i < (takeAll ? maxLoop : 1); i++)
                {
                    slot.DecrementSlot();
                    numberItemsMoving++;
                }
                if (slot == resultSlot && slot.GetNbItems() == 0)
                {
                    slot.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (slot.IsItemSameAs(itemMoving))
            {
                int maxLoop = slot.GetNbItems();
                for (int i = 0; i < (takeAll ? maxLoop : 1); i++)
                {
                    slot.DecrementSlot();
                    numberItemsMoving++;
                }
                if (slot == resultSlot && slot.GetNbItems() == 0)
                {
                    slot.gameObject.SetActive(false);
                }
            }
            else
            {
                Item inSlot = slot.GetItem();
                int nbInSlot = slot.GetNbItems();

                slot.ResetSlot();
                slot.AddItem(itemMoving, numberItemsMoving);

                itemMoving = inSlot;
                numberItemsMoving = nbInSlot;
            }
        }
        RefreshWaterColor();
        helper.Refresh(itemMoving, numberItemsMoving);
    }

    public void PlaceItemInCraftingSlot(CraftingSlot slot)
    {
        if (itemMoving == null) return;
        bool takeAll = Input.GetKey(KeyCode.LeftShift);

        Player player = GameManager.player;

        if (slot.IsItemSameAs(itemMoving) ||
            slot.GetItem() == null)
        {


            slot.AddItem(itemMoving, takeAll ? numberItemsMoving : 1);
            numberItemsMoving -= takeAll ? numberItemsMoving : 1;
            if (numberItemsMoving == 0)
            {
                itemMoving = null;
            }
        }

        RefreshWaterColor();
        helper.Refresh(itemMoving, numberItemsMoving);
    }


    public void CraftItem()
    {

        if (resultSlot.GetItem() != null) return;

        Item item1;
        Item item2;
        foreach (Recipe recipe in GameManager.recipeManager.GetAllRecipeInMachine(currentMachine))
        {
            item1 = GameManager.instance.GetItem(recipe.item1Name);
            item2 = GameManager.instance.GetItem(recipe.item2Name);
            if ((Item1CorrespondsTo(item1, recipe.item1Count) && Item2CorrespondsTo(item2, recipe.item2Count)) ||
            (Item2CorrespondsTo(item1, recipe.item1Count) && Item1CorrespondsTo(item2, recipe.item2Count)))
            {
                resultSlot.gameObject.SetActive(true);
                resultSlot.AddItem(GameManager.instance.GetItem(recipe.resultName), recipe.resultCount);
                slot1.ResetSlot();
                slot2.ResetSlot();

                foreach (Quest q in GameManager.player.GetAllQuests())
                {
                    if (q.currentPhase >= 0 && q.currentPhase < q.maxPhases &&
                    q.GetCraftRequirementForCurrentPhase().Equals(recipe.resultName))
                    {
                        GameManager.player.IncrementQuest(q.questID);
                    }
                }


                return;
            }
        }
    }


    void RefreshWaterColor()
    {
        int correctIndex = ((int)currentMachine) - 1;
        if (slot1.GetItem() == null && slot2.GetItem() == null)
        {
            machinesGfx[correctIndex].ResetWaterColor();
        }
        else
        {
            Color col1 = slot1.GetAverageColorOfItem() * slot1.GetNbItems();
            Color col2 = slot2.GetAverageColorOfItem() * slot2.GetNbItems();
            int total = slot1.GetNbItems() + slot2.GetNbItems();

            if (col1.a == 0)
            {
                machinesGfx[correctIndex].SetWaterColor(col2 / slot2.GetNbItems());
            }
            else if (col2.a == 0)
            {
                machinesGfx[correctIndex].SetWaterColor(col1 / slot1.GetNbItems());
            }
            else
            {
                machinesGfx[correctIndex].SetWaterColor(new Color((col1.r + col2.r) / total,
                (col1.g + col2.g) / total,
                (col1.b + col2.b) / total,
                1));
            }


        }
    }


    public void Initialize()
    {
        slot1.RefreshSlot();
        slot2.RefreshSlot();
        resultSlot.RefreshSlot();
        resultSlot.gameObject.SetActive(false);
    }
}
