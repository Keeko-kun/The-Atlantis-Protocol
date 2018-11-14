using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPauseGame : MonoBehaviour {

    public FadeUI pauseMenu;

	void Start () {
		
	}
	
	void Update () {
		if (pauseMenu && Globals.GetButtonDown("Menu"))
        {
            if (pauseMenu.visible && Globals.inputType == InputType.Menu)
            {
                Globals.inputType = InputType.Gameplay;
                Time.timeScale = 1;
                pauseMenu.visible = false;
                GetComponent<PreventInput>().InputProhibited = false;
            }
            else
            {
                if (GetComponent<PreventInput>().InputProhibited)
                    return;

                Globals.inputType = InputType.Menu;
                Time.timeScale = 0;
                pauseMenu.visible = true;
                GetComponent<PreventInput>().InputProhibited = true;
            }
        }
	}
}
