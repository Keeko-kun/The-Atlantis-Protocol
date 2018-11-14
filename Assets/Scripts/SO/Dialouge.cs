using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialouge", menuName = "Dialouge", order = 1)]
public class Dialouge : ScriptableObject {

    [Header("Standard Settings")]
    public string ID;
    public List<int> setEventsWhenDone;

    [Header("Items")]
    public bool givesItem;
    public bool setsItem;
    public Item item;

    [Header("Conditions")]
    public List<ChangeDialouge> conditions;

}
