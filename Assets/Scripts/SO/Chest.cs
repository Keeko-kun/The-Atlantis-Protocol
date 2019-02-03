using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest", menuName = "Chest", order = 1)]
public class Chest : ScriptableObject {

    [Header("General Settings")]
    public int eventID;
    public ChestContents contents;

    [HideInInspector]
    public int amount;
    [HideInInspector]
    public GameObject enemySpawner;
    [HideInInspector]
    public Item item;

}

#if UNITY_EDITOR
[CustomEditor(typeof(Chest))]
public class Chest_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Chest script = (Chest)target;

        switch (script.contents)
        {
            case ChestContents.Money:
                EditorGUILayout.LabelField("Money Options", EditorStyles.boldLabel);
                script.amount = EditorGUILayout.IntField("Amount", script.amount);
                break;
            case ChestContents.Enemy:
                EditorGUILayout.LabelField("Enemy Options", EditorStyles.boldLabel);
                script.enemySpawner = EditorGUILayout.ObjectField("Enemy Spawner", script.enemySpawner, typeof(GameObject), true) as GameObject;
                break;
            case ChestContents.Item:
                EditorGUILayout.LabelField("Item Options", EditorStyles.boldLabel);
                script.item = EditorGUILayout.ObjectField("Enemy Spawner", script.item, typeof(Item), true) as Item;
                break;
            case ChestContents.Nothing:
                break;
        }
    }
}
#endif
