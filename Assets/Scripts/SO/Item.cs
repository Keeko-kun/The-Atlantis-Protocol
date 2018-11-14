using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Item", menuName = "Item", order = 1)]
public class Item : ScriptableObject {

    [Header("Item Settings")]
    public ItemType type;
    public string itemName;
    public string description;
    public Sprite sprite;

    [HideInInspector]
    public int baseStrength;
    [HideInInspector]
    public GameObject hitboxesPrefab;
    [HideInInspector]
    public bool stackable;
    [HideInInspector]
    public int maxStack;
    [HideInInspector]
    public UseableType useableType;
    [HideInInspector]
    public EffectType effectType;
    [HideInInspector]
    public int effectAmount;
    [HideInInspector]
    public GameObject prefab;
}


#if UNITY_EDITOR
[CustomEditor(typeof(Item))]
public class Item_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Item script = (Item)target;

        switch (script.type)
        {
            case ItemType.Melee:
                EditorGUILayout.LabelField("Melee Item Options", EditorStyles.boldLabel);
                script.baseStrength = EditorGUILayout.IntField("Base Strength", script.baseStrength);
                script.hitboxesPrefab = EditorGUILayout.ObjectField("Hitboxes Prefab", script.hitboxesPrefab, typeof(GameObject), true) as GameObject;
                break;

            case ItemType.Special:
                EditorGUILayout.LabelField("Special Item Options", EditorStyles.boldLabel);
                script.baseStrength = EditorGUILayout.IntField("Base Strength", script.baseStrength);
                script.prefab = EditorGUILayout.ObjectField("Special Prefab", script.prefab, typeof(GameObject), true) as GameObject;
                break;

            case ItemType.Key:
                script.stackable = EditorGUILayout.Toggle("Stackable", script.stackable);

                if (script.stackable)
                {
                    script.maxStack = EditorGUILayout.IntField("Max Stack", script.maxStack);
                }
                break;

            case ItemType.Useable:
                EditorGUILayout.LabelField("Useable Item Options", EditorStyles.boldLabel);
                script.maxStack = EditorGUILayout.IntField("Max Stack", script.maxStack);
                script.useableType = (UseableType)EditorGUILayout.EnumPopup("Type of Useable", script.useableType);

                if (script.useableType == UseableType.Spawn)
                {
                    EditorGUILayout.LabelField("Spawn Useable Options", EditorStyles.boldLabel);
                    script.prefab = EditorGUILayout.ObjectField("Useable Prefab", script.prefab, typeof(GameObject), true) as GameObject;
                }
                else
                {
                    EditorGUILayout.LabelField("Effect Useable Options", EditorStyles.boldLabel);
                    script.effectType = (EffectType)EditorGUILayout.EnumPopup("Type of Effect", script.effectType);
                    script.effectAmount = EditorGUILayout.IntField("Effect Strength", script.effectAmount);
                }

                break;
        }
    }
}
#endif