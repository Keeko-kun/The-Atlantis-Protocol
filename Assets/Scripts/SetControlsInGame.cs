using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetControlsInGame : MonoBehaviour {

    public Control jump;
    public Control attack;
    public Control special;
    public Control action;
    public Control crouch;
    public Control item;
    public Control map;
    public Control menu;

    void Start () {
        UpdateText();
    }

    public void UpdateText()
    {
        jump.keyboard.text = Globals.GetButtons("Jump")[0];
        jump.gamepad.text = Globals.GetButtons("Jump")[1];
        attack.keyboard.text = Globals.GetButtons("Attack")[0];
        attack.gamepad.text = Globals.GetButtons("Attack")[1];
        special.keyboard.text = Globals.GetButtons("Special")[0];
        special.gamepad.text = Globals.GetButtons("Special")[1];
        action.keyboard.text = Globals.GetButtons("Action")[0];
        action.gamepad.text = Globals.GetButtons("Action")[1];
        crouch.keyboard.text = Globals.GetButtons("Crouch")[0];
        crouch.gamepad.text = Globals.GetButtons("Crouch")[1];
        item.keyboard.text = Globals.GetButtons("Item")[0];
        item.gamepad.text = Globals.GetButtons("Item")[1];
        map.keyboard.text = Globals.GetButtons("Map")[0];
        map.gamepad.text = Globals.GetButtons("Map")[1];
        menu.keyboard.text = Globals.GetButtons("Menu")[0];
        menu.gamepad.text = Globals.GetButtons("Menu")[1];
    }
}

[System.Serializable]
public class Control{
    public TextMeshPro keyboard;
    public TextMeshPro gamepad;
}
