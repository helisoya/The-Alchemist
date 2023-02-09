using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Player
{
    public static GameObject body;
    public InventoryItemContainer[] items;

    public int bagSize;

    public int maxInHotBar;

    public int currentSlot;

    public float speed;

    public int gold;

    public int day;

    public int month;

    public int year;

    public Vector2 startPos;

    [SerializeField] public List<Quest> quests;

    public Player()
    {
        speed = 5;
        maxInHotBar = 8;
        currentSlot = 0;
        bagSize = 24;
        gold = 0;
        day = 0;
        month = 0;
        year = 1;
        startPos = Vector2.positiveInfinity;
        items = new InventoryItemContainer[bagSize];
        quests = new List<Quest>();
    }

    public bool AddItemToSlot(Item item, int nb, int slot)
    {

        if (items[slot] == null)
        {
            items[slot] = new InventoryItemContainer(item, nb);
            return true;
        }

        if (!items[slot].IsItemSameAs(item))
        {
            return false;
        }

        items[slot].AddToCount(nb);
        return true;
    }

    public void DeleteSlot(int slot)
    {
        items[slot] = null;
    }

    public Item GetItemFromSlot(int slot)
    {
        if (items[slot] == null) return null;
        return items[slot].actualItem;
    }

    public int GetNbItemsInSlot(int slot)
    {
        if (items[slot] == null) return 0;
        return items[slot].itemCount;
    }

    public bool IsItemInSlotSameAs(int slot, Item itemRef)
    {
        if (items[slot] == null)
        {
            return false;
        }
        return items[slot].IsItemSameAs(itemRef);
    }

    public void DecrementSlot(int slot)
    {
        if (items[slot] != null)
        {
            items[slot].itemCount--;
            if (items[slot].itemCount == 0)
            {
                items[slot] = null;
            }
        }
    }

    public bool AddItem(Item item, int number)
    {
        for (int i = 0; i < bagSize; i++)
        {
            if (items[i] != null)
            {
                if (items[i].IsItemSameAs(item))
                {
                    AddItemToSlot(item, number, i);
                    return true;
                }
            }
        }

        for (int i = 0; i < bagSize; i++)
        {
            if (items[i] != null && items[i].itemRef == null)
            {
                DeleteSlot(i);
            }
            if (items[i] == null)
            {
                AddItemToSlot(item, number, i);
                return true;
            }
        }
        return false;
    }

    public bool CanAddItem(Item item)
    {
        for (int i = 0; i < bagSize; i++)
        {
            if (items[i] == null || items[i].IsItemSameAs(item))
            {
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(string name)
    {
        foreach (InventoryItemContainer slot in items)
        {
            if (slot != null && slot.IsItemSameAs(GameManager.instance.GetItem(name)))
            {
                slot.itemCount--;
                return true;
            }
        }
        return false;
    }


    public Quest GetQuest(string id)
    {
        foreach (Quest q in quests)
        {
            if (id.Equals(q.questID))
            {
                return q;
            }
        }
        return null;
    }

    public List<Quest> GetAllQuests()
    {
        return quests;
    }


    public void IncrementQuest(string quest)
    {
        Quest q = GetQuest(quest);
        q.currentPhase++;
        if (q.currentPhase == q.maxPhases)
        {
            gold += q.goldReward;
            PlayerInfo.instance.RefreshGold();
        }
    }

    public void LoadQuests()
    {
        quests = new List<Quest>();
        List<string> fileContent = FileManager.ReadTextAsset(Resources.Load<TextAsset>("quests"));
        string line;
        string[] split;
        Quest currentItem = null;

        for (int i = 0; i < fileContent.Count; i++)
        {
            line = fileContent[i];
            if (line.StartsWith("#")) continue;

            if ((line.StartsWith("[") && line.EndsWith("]")) || string.IsNullOrWhiteSpace(line))
            {
                if (currentItem != null)
                {
                    quests.Add(currentItem);
                    currentItem = null;
                }

                if (!string.IsNullOrWhiteSpace(line))
                {
                    currentItem = new Quest(line.Substring(1, line.Length - 2));
                }
                continue;
            }


            split = line.Split(" = ");

            if (split.Length != 2)
            {
                Debug.Log("Error on line " + line + ". There should be only one = .");
                continue;
            }

            if (split[0].EndsWith(" "))
            {
                split[0] = split[0].Substring(0, split[0].Length - 1);
            }
            if (split[1].EndsWith(" "))
            {
                split[1] = split[1].Substring(0, split[1].Length - 1);
            }

            switch (split[0].Split("_")[0])
            {
                case "Reward":
                    currentItem.goldReward = int.Parse(split[1]);
                    break;
                case "MaxPhases":
                    currentItem.maxPhases = int.Parse(split[1]);
                    currentItem.craftRequirement = new string[currentItem.maxPhases];
                    break;
                case "Phase":
                    currentItem.craftRequirement[int.Parse(split[0].Split("_")[1])] = split[1];
                    break;
            }
        }
        if (currentItem != null)
        {
            quests.Add(currentItem);
            currentItem = null;
        }

    }
}
