using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveProgress : MonoBehaviour
{
    public string locationName;
    public string spawnID;

    private int slot;

    public IEnumerator SaveGame(GameObject player)
    {
        yield return null;

        slot = DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().SaveSlot;

        CharacterHealth ch = player.GetComponent<CharacterHealth>();
        CharacterInventory ci = player.GetComponent<CharacterInventory>();

        PlayerPrefsX.SetBool("tfpSave_" + slot, true);
        PlayerPrefs.SetString("tfpLocation_" + slot, locationName);
        PlayerPrefs.SetString("tfpScene_" + slot, SceneManager.GetActiveScene().name);
        PlayerPrefs.SetString("tfpSpawn_" + slot, spawnID);
        PlayerPrefs.SetFloat("tfpHealth_" + slot, ch.health.currentHealth);
        PlayerPrefs.SetFloat("tfpMaxHealth_" + slot, ch.health.maxHealth);
        PlayerPrefsX.SetBoolArray("tfpEvents_" + slot, Globals.events);
        ci.inv.ListAllItems();
        PlayerPrefsX.SetStringArray("tfpInvItems_" + slot, ci.inv.AllItemsName().ToArray());
        PlayerPrefsX.SetIntArray("tfpInvStack_" + slot, ci.inv.AllItemsStack().ToArray());
        PlayerPrefsX.SetBoolArray("tfpInvEquipped_" + slot, ci.inv.AllItemsEquipped().ToArray());

        PlayerPrefs.Save();
    }
}
