using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour {

    public RectTransform maxHealth;
    public RectTransform currentHealth;

    public void UpdateHealth(float max, float current)
    {
        maxHealth.sizeDelta = new Vector2(max, maxHealth.sizeDelta.y);
        currentHealth.sizeDelta = new Vector2(current, currentHealth.sizeDelta.y);
    }

}
