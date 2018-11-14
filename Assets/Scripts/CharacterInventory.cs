using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterInventory : MonoBehaviour
{

    public InventoryUI invUI;

    public Inventory inv;

    public Item coin;

    void Awake()
    {
        inv.CreateList(coin);
        GetInstance();
    }

    private void SaveInstance()
    {
        DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().Inventory = inv;
        invUI.UpdateItems();
    }

    public void GetInstance()
    {
        if (DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().Inventory != null)
        {
            inv = DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().Inventory;
        }
    }

    public void SetItem(Item item)
    {
        PlayerItem ItemToSet = null;
        switch (item.type)
        {
            case ItemType.Melee:

                foreach (PlayerItem meleeItem in inv.melee)
                {
                    if (meleeItem.item == item)
                    {
                        ItemToSet = meleeItem;
                        break;
                    }
                }
                inv.currentMelee = ItemToSet;
                break;
            case ItemType.Special:
                foreach (PlayerItem specialItem in inv.special)
                {
                    if (specialItem.item == item)
                    {
                        ItemToSet = specialItem;
                        break;
                    }
                }
                inv.currentSpecial = ItemToSet;
                break;
            case ItemType.Useable:
                foreach (PlayerItem useableItem in inv.useable)
                {
                    if (useableItem.item == item)
                    {
                        ItemToSet = useableItem;
                        break;
                    }
                }
                inv.currentUseable = ItemToSet;
                break;
        }

        SaveInstance();
    }

    public void GiveItem(Item item, int stack)
    {
        PlayerItem itemToAdd = new PlayerItem(item, stack);
        switch (item.type)
        {
            case ItemType.Melee:
                inv.AddItem(item, stack, inv.melee);
                break;
            case ItemType.Special:
                inv.AddItem(item, stack, inv.special);
                break;
            case ItemType.Key:
                inv.AddItem(item, stack, inv.key);
                break;
            case ItemType.Useable:
                inv.AddItem(item, stack, inv.useable);
                break;
        }

        SaveInstance();
    }

    public void RemoveItem(Item item)
    {
        PlayerItem itemToRemove = null;
        switch (item.type)
        {
            case ItemType.Melee:
                foreach (PlayerItem meleeItem in inv.melee)
                {
                    if (meleeItem.item == item)
                    {
                        itemToRemove = meleeItem;
                        break;
                    }
                }
                inv.melee.Remove(itemToRemove);
                break;
            case ItemType.Special:
                foreach (PlayerItem specialItem in inv.special)
                {
                    if (specialItem.item == item)
                    {
                        itemToRemove = specialItem;
                        break;
                    }
                }
                inv.special.Remove(itemToRemove);
                break;
            case ItemType.Key:
                foreach (PlayerItem keyItem in inv.key)
                {
                    if (keyItem.item == item)
                    {
                        itemToRemove = keyItem;
                        break;
                    }
                }
                inv.special.Remove(itemToRemove);
                break;
            case ItemType.Useable:
                foreach (PlayerItem useableItem in inv.useable)
                {
                    if (useableItem.item == item)
                    {
                        itemToRemove = useableItem;
                        break;
                    }
                }
                inv.useable.Remove(itemToRemove);
                break;
        }

        SaveInstance();
    }
}