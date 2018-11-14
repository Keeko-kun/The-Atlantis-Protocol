using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[System.Serializable]
public class Inventory
{
    public PlayerItem currentMelee;
    public PlayerItem currentSpecial;
    public PlayerItem currentUseable;

    private List<PlayerItem> playerItems = new List<PlayerItem>();

    public List<PlayerItem> melee { get; private set; }
    public List<PlayerItem> special { get; private set; }
    public List<PlayerItem> key { get; private set; }
    public List<PlayerItem> useable { get; private set; }

    public void CreateList(Item coin)
    {
        melee = new List<PlayerItem>();
        special = new List<PlayerItem>();
        key = new List<PlayerItem>();
        useable = new List<PlayerItem>();

        PlayerItem pi = new PlayerItem(coin, 0);
        key.Add(pi);
    }

    public void AddItem(Item item, int stack, List<PlayerItem> type)
    {

        if (type.Exists(x => x.item == item))
        {
            type.Find(x => x.item == item).stack += stack;
        }
        else
        {
            PlayerItem itemToAdd = new PlayerItem(item, stack);
            type.Add(itemToAdd);
        }
    }

    public void ListAllItems()
    {
        playerItems = new List<PlayerItem>();

        playerItems.AddRange(melee);
        playerItems.AddRange(special);
        playerItems.AddRange(useable);
        playerItems.AddRange(key);
    }

    public List<string> AllItemsName()
    {
        List<string> items = new List<string>();

        foreach (PlayerItem pi in playerItems)
        {
            items.Add(pi.item.itemName);
        }

        return items;
    }

    public List<int> AllItemsStack()
    {
        List<int> items = new List<int>();

        foreach (PlayerItem pi in playerItems)
        {
            items.Add(pi.stack);
        }

        return items;
    }

    public List<bool> AllItemsEquipped()
    {
        List<bool> items = new List<bool>();

        foreach (PlayerItem pi in playerItems)
        {
            items.Add(IsEquipped(pi.item));
        }

        return items;
    }

    public bool IsEquipped(Item item)
    {
        switch (item.type)
        {
            case ItemType.Melee:
                if (currentMelee == null)
                    return false;
                else if (currentMelee.item == item)
                    return true;
                else
                    return false;
            case ItemType.Special:
                if (currentSpecial == null)
                    return false;
                else if (currentSpecial.item == item)
                    return true;
                else
                    return false;
            case ItemType.Useable:
                if (currentUseable == null)
                    return false;
                else if (currentUseable.item == item)
                    return true;
                else
                    return false;
        }

        return false;
    }
}

[System.Serializable]
public class PlayerItem
{
    public Item item;
    public int stack;

    public PlayerItem(Item item, int stack)
    {
        this.item = item;
        this.stack = stack;
    }
}
