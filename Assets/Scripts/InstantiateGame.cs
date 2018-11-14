using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateGame : MonoBehaviour {

    [Header("Important Settings")]
    public Transform player;
    public FadeUI darkness;

    [Header("Spawn Settings")]
    public string spawnID;
    public List<SpawnLocation> spawnLocations;

    private void Awake()
    {
        if (darkness)
        {
            darkness.GetComponent<CanvasGroup>().alpha = 1;
            darkness.visible = true;
        }
    }

    private void Start()
    {
        StartCoroutine(DoSpawnPlayer());
    }

    private IEnumerator DoSpawnPlayer()
    {
        Time.timeScale = 0;
        SpawnLocation targetLocation = null;
        player.GetComponent<PreventInput>().InputProhibited = true;

        if (DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().sceneSpawn != null)
        {
            spawnID = DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().sceneSpawn.spawnID;
        }

        if (DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().SpawnID != "")
        {
            spawnID = DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().SpawnID;
            DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().SpawnID = "";
        }

        foreach (SpawnLocation location in spawnLocations)
        {
            if (location.name == spawnID)
            {
                location.roomMaster.gameObject.SetActive(true);
                targetLocation = location;
            }
            else
            {
                location.roomMaster.gameObject.SetActive(false);
            }

        }
        if (targetLocation == null)
        {
            Debug.LogError("There is no TargetLocation with the name: " + spawnID);
            yield break;
        }

        RoomTransition targetRoom = targetLocation.location.GetComponent<RoomTransition>();

        yield return StartCoroutine(targetRoom.DoTransition(targetLocation.location, targetLocation.roomMaster, player));
    }

}

[System.Serializable]
public class SpawnLocation
{
    public string name;
    public RoomMaster roomMaster;
    public Transform location;
}
