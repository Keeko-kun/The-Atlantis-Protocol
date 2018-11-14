using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy", order = 1)]
public class Enemy : ScriptableObject {

    public float maxHealth;
    public int goldDropMin;
    public int goldDropMax;
    public int attackStrength;
    public int contactDamage;

}
