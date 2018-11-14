using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class LoadSaveFile : MonoBehaviour
{

    public Item coin;
    public List<Item> allItems;

    private StoreSceneInfo store;

    void Start()
    {
        store = GetComponent<StoreSceneInfo>();
    }


    public IEnumerator LoadFile(int slot)
    {
        string sceneName;

        Globals.events = PlayerPrefsX.GetBoolArray("tfpEvents_" + slot);

        store.SaveSlot = slot;

        Health health = new Health();
        health.currentHealth = PlayerPrefs.GetFloat("tfpHealth_" + slot);
        health.maxHealth = PlayerPrefs.GetFloat("tfpMaxHealth_" + slot);

        store.Health = health;

        sceneName = PlayerPrefs.GetString("tfpScene_" + slot);
        store.SpawnID = PlayerPrefs.GetString("tfpSpawn_" + slot);

        Inventory inventory = new Inventory();
        inventory.CreateList(coin);

        string[] items = PlayerPrefsX.GetStringArray("tfpInvItems_" + slot);
        int[] stack = PlayerPrefsX.GetIntArray("tfpInvStack_" + slot);
        bool[] equipped = PlayerPrefsX.GetBoolArray("tfpInvEquipped_" + slot);

        for (int i = 0; i < items.Length; i++)
        {
            Item item = new Item();
            foreach (Item it in allItems)
            {
                if (items[i] == it.itemName)
                    item = it;
            }

            switch (item.type)
            {
                case ItemType.Melee:
                    inventory.AddItem(item, stack[i], inventory.melee);
                    if (equipped[i])
                        inventory.currentMelee = new PlayerItem(item, stack[i]);
                    break;
                case ItemType.Special:
                    inventory.AddItem(item, stack[i], inventory.special);
                    if (equipped[i])
                        inventory.currentSpecial = new PlayerItem(item, stack[i]);
                    break;
                case ItemType.Key:
                    inventory.AddItem(item, stack[i], inventory.key);
                    break;
                case ItemType.Useable:
                    inventory.AddItem(item, stack[i], inventory.useable);
                    if (equipped[i])
                        inventory.currentUseable = new PlayerItem(item, stack[i]);
                    break;
            }
        }

        store.Inventory = inventory;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        while (!op.isDone)
        {
            yield return null;
        }

    }
}
