using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public InventoryBar keyItems;
    public InventoryBar meleeItems;
    public InventoryBar specialItems;
    public InventoryBar useableItems;

    private CharacterInventory inventory;
    private int[] numbers = new int[3];

    void Start()
    {
        inventory = GetComponent<ControlMainMenu>().player.GetComponent<CharacterInventory>();

        UpdateItems();
    }

    public void UpdateItems()
    {
        SetItemBar(keyItems, inventory.inv.key, 0);
        SetItemBar(meleeItems, inventory.inv.melee, 0);
        SetItemBar(specialItems, inventory.inv.special, 0);
        SetItemBar(useableItems, inventory.inv.useable, 0);
    }

    private int SetItemBar(InventoryBar bar, List<PlayerItem> items, int offset)
    {
        if (items.Count == 0)
        {
            return 0;
        }

        if (items.Count == 1)
        {
            numbers = new int[3] { 0, 0, 0 };
        }
        else if (items.Count == 2)
        {
            if (offset != 0 && numbers[1] == 0)
                numbers = new int[3] { 0, 1, 0 };
            else
                numbers = new int[3] { 1, 0, 1 };
        }
        else if (items.Count >= 3)
        {
            int itemCount = items.Count - 1;

            if (offset == 0)
            {
                numbers = new int[3] { 0, 1, 2 };
            }
            else if (offset == 1)
            {              
                for (int i = 0; i < 3; i++)
                {
                    int number = numbers[i] - 1;
                    if (number < 0)
                        number = itemCount;
                    numbers[i] = number;
                }
            }
            else if (offset == -1)
            {
                for (int i = 0; i < 3; i++)
                {
                    int number = numbers[i] + 1;
                    if (number > itemCount)
                        number = 0;
                    numbers[i] = number;
                }
            }

        }

        bar.leftSlot.sprite = items[numbers[0]].item.sprite;
        SetEquipped(items[numbers[0]], bar.leftSlot);
        bar.middleSlot.sprite = items[numbers[1]].item.sprite;
        SetEquipped(items[numbers[1]], bar.middleSlot);
        bar.rightSlot.sprite = items[numbers[2]].item.sprite;
        SetEquipped(items[numbers[2]], bar.rightSlot);

        SetItemText(bar, items);

        if (offset == 0)
            GetComponent<ControlMainMenu>().ResetPosition();

        return numbers[1];
    }

    private void ClearItemText(InventoryBar bar)
    {
        bar.itemName.text = "None";
        bar.description.text = "There is no item.";
    }

    private void SetItemText(InventoryBar bar, List<PlayerItem> items)
    {
        bar.itemName.text = items[numbers[1]].item.itemName;
        bar.description.text = items[numbers[1]].item.description;
    }

    private void SetEquipped(PlayerItem playerItem, Image slot)
    {
        TextMeshProUGUI[] texts = slot.GetComponentsInChildren<TextMeshProUGUI>(true);

        TextMeshProUGUI equipped = null;

        foreach(TextMeshProUGUI t in texts)
        {
            if (t.gameObject.name == "Equipped")
                equipped = t;
        }

        bool isEquipped = inventory.inv.IsEquipped(playerItem.item);

        switch (playerItem.item.type)
        {
            case ItemType.Melee:
                equipped.gameObject.SetActive(isEquipped);
                break;
            case ItemType.Special:
                equipped.gameObject.SetActive(isEquipped);
                break;
            case ItemType.Useable:
                equipped.gameObject.SetActive(isEquipped);

                SetStack(playerItem, slot);
                break;
            case ItemType.Key:
                SetStack(playerItem, slot);
                break;
        }
    }

    private void SetStack(PlayerItem playerItem, Image slot)
    {
        TextMeshProUGUI[] texts = slot.GetComponentsInChildren<TextMeshProUGUI>(true);

        TextMeshProUGUI stack = null;

        foreach (TextMeshProUGUI t in texts)
        {
            if (t.gameObject.name == "Stack")
                stack = t;
        }

        if (playerItem.item.type == ItemType.Key && playerItem.item.stackable == false)
        {
            stack.gameObject.SetActive(false);
        }
        else
        {
            stack.gameObject.SetActive(true);
            stack.text = playerItem.stack.ToString();
        }

    }

    public void MoveVertical(ItemType type)
    {
        switch (type)
        {
            case ItemType.Melee:
                if (inventory.inv.melee.Count == 0)
                    ClearItemText(meleeItems);
                else
                    SetItemText(meleeItems, inventory.inv.melee);
                break;
            case ItemType.Special:
                if (inventory.inv.special.Count == 0)
                    ClearItemText(specialItems);
                else
                    SetItemText(specialItems, inventory.inv.special);
                break;
            case ItemType.Useable:
                if (inventory.inv.useable.Count == 0)
                    ClearItemText(useableItems);
                else
                    SetItemText(useableItems, inventory.inv.useable);
                break;
        }
    }

    public int MoveBar(ItemType type, Direction dir, int currentX)
    {
        switch (type)
        {
            case ItemType.Melee:
                return SetItemBar(meleeItems, inventory.inv.melee, (int)dir);
            case ItemType.Special:
                return SetItemBar(specialItems, inventory.inv.special, (int)dir);
            case ItemType.Key:
                return SetItemBar(keyItems, inventory.inv.key, (int)dir);
            case ItemType.Useable:
                return SetItemBar(useableItems, inventory.inv.useable, (int)dir);
        }
        return 0;
    }
}

[System.Serializable]
public class InventoryBar
{
    public ItemType type;
    public Image leftSlot;
    public Image middleSlot;
    public Image rightSlot;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI description;
}
