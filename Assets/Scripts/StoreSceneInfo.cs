using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSceneInfo : MonoBehaviour {

    public SceneSpawn sceneSpawn;

    private void Start()
    {
        SaveSlot = 0;
        SpawnID = "";
    }

    public Inventory Inventory { get; set; }
    public Health Health { get; set; }
    public int SaveSlot { get; set; }
    public string SpawnID { get; set; }
}
