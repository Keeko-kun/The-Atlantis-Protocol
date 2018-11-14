using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Menu", menuName = "Menu", order = 1)]
public class Menu : ScriptableObject {

    [Header("Cursor Settings")]
    public bool verticalEnabled;
    public int verticalAmount;
    public float verticalOffset;

    public bool horizontalEnabled;
    public int horizontalAmount;
    public float horizontalOffset;

    [Header("Inventory")]
    public bool menuIsInventory;


}
