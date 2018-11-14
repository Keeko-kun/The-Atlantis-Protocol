using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour {

    [Header("Type Settings")]
    public Transition transitionType;
    public Direction exitDirection;

    [Header("Destination")]
    public int sceneIndex;

    [Header("UI")]
    public FadeUI darkness;

}
