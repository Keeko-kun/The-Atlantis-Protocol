using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class ControlMainMenu : MonoBehaviour
{

    public List<MenuScreen> screens;
    public SetControlsInGame controlsGame;
    public SetControlInGUI controlsGUI;
    public PreventInput player;
    public FadeGameWorld darknessGame;
    public SceneSpawn gameStartScene;

    private float xPos = 0;
    private float yPos = 0;
    private MenuScreen currentScreen;
    private int currentID;
    private bool rapidPrevention = false;
    private InventoryUI invUI;

    void Start()
    {
        invUI = GetComponent<InventoryUI>();
        currentID = 0;
        currentScreen = screens[0];

        if (currentScreen.screen.GetComponent<RectTransform>() == null)
        {
            currentScreen.cursorPos = currentScreen.cursor.position;
        }
        else
        {
            currentScreen.cursorPos = currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition;
        }
    }

    void Update()
    {
        if (Globals.inputType != InputType.Menu)
            return;

        int buttonCountVertical = currentScreen.menuSettings.verticalAmount;
        int buttonCountHorizontal = currentScreen.menuSettings.horizontalAmount;

        if (!rapidPrevention)
        {
            HandleVertical(buttonCountVertical);

            if (currentScreen.menuSettings.menuIsInventory)
                HandleHorizontalInventory();
            else
                HandleHorizontal(buttonCountHorizontal);
        }

        if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
            rapidPrevention = false;

        ButtonInput();
    }

    private void ButtonInput()
    {
        if (Globals.GetButtonDown("Jump"))
        {
            HandleButtonPress();
        }
        else if (Globals.GetButtonDown("Special"))
        {
            ScreenTransition(0);
        }
    }

    private void HandleButtonPress()
    {
        foreach (MenuButton btn in currentScreen.buttons)
        {
            if (btn.coordinates.x == xPos && btn.coordinates.y == yPos)
            {
                if (btn.isActive == false)
                    return;

                switch (btn.buttonType)
                {
                    case ButtonType.GoToMenu:
                        ScreenTransition(btn.screenID);
                        break;
                    case ButtonType.ChangeInputGamepad:
                        StartCoroutine(ChangeInputGamepad(btn.command));
                        break;
                    case ButtonType.ChangeInputKeyboard:
                        StartCoroutine(ChangeInputKeyboard(btn.command));
                        break;
                    case ButtonType.ContinueGame: //Only on UI plz.
                        StartCoroutine(ContinueGameplay());
                        break;
                    case ButtonType.LoadFile:
                        StartCoroutine(LoadSaveFile(int.Parse(btn.command)));
                        break;
                    case ButtonType.NewGame:
                        StartCoroutine(NewGame(btn.command));
                        break;
                }
            }
        }
    }

    private IEnumerator NewGame(string slot)
    {
        if (PlayerPrefs.HasKey("tfpSave_" + slot))
        {
            PlayerPrefs.DeleteKey("tfpSave_" + slot);
            PlayerPrefs.DeleteKey("tfpLocation_" + slot);
            PlayerPrefs.DeleteKey("tfpHealth_" + slot);
            PlayerPrefs.DeleteKey("tfpMaxHealth_" + slot);
            PlayerPrefs.DeleteKey("tfpSpawn_" + slot);
            PlayerPrefs.DeleteKey("tfpScene_" + slot);
            PlayerPrefs.DeleteKey("tfpInvItems_" + slot);
            PlayerPrefs.DeleteKey("tfpInvStack_" + slot);
            PlayerPrefs.DeleteKey("tfpInvEquipped_" + slot);
            PlayerPrefs.DeleteKey("tfpEvents_" + slot);
        }

        darknessGame.visible = true;

        yield return new WaitForSecondsRealtime(2f);

        DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().sceneSpawn = gameStartScene;
        DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().SaveSlot = int.Parse(slot);

        AsyncOperation op = SceneManager.LoadSceneAsync(gameStartScene.sceneName.ToString());

        while (!op.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator LoadSaveFile(int slot)
    {
        if (PlayerPrefs.HasKey("tfpSave_" + slot.ToString()) == false || PlayerPrefsX.GetBool("tfpSave_" + slot.ToString()) == false)
            yield break;

        Globals.inputType = InputType.Gameplay;

        darknessGame.visible = true;

        yield return new WaitForSecondsRealtime(2f);

        yield return StartCoroutine(DontDestroy.GetInstance().GetComponent<LoadSaveFile>().LoadFile(slot));
    }

    private IEnumerator ContinueGameplay()
    {
        Globals.inputType = InputType.Gameplay;
        Time.timeScale = 1;
        GetComponent<FadeUI>().visible = false;
        yield return null;
        player.InputProhibited = false;
    }

    private IEnumerator ChangeInputKeyboard(string command)
    {
        Globals.inputType = InputType.EnterInputKeyboard;

        KeyCode? newKeyCode = null;

        float timer = 0;

        while (newKeyCode == null && timer < 3)
        {
            yield return null;
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    if (kcode.ToString().Contains("Joystick"))
                        continue;

                    newKeyCode = kcode;
                    break;
                }
            }
            timer += Time.unscaledDeltaTime;
        }

        if (timer > 3)
        {
            Globals.inputType = InputType.Menu;
            yield break;
        }

        Globals.ChangeInputs(command, (KeyCode)newKeyCode);
        UpdateControls();

        Globals.inputType = InputType.Menu;
    }

    private IEnumerator ChangeInputGamepad(string command)
    {
        Globals.inputType = InputType.EnterInputGamepad;

        GamePad? newGamepadCode = null;

        float timer = 0;

        while (newGamepadCode == null && timer < 3)
        {
            yield return null;
            foreach (GamePad gcode in Enum.GetValues(typeof(GamePad)))
            {
                if (Input.GetButtonDown(gcode.ToString()))
                {
                    newGamepadCode = gcode;
                }
            }
            timer += Time.unscaledDeltaTime;
        }

        if (timer > 3)
        {
            Globals.inputType = InputType.Menu;
            yield break;
        }

        Globals.ChangeInputs(command, (GamePad)newGamepadCode);
        UpdateControls();

        Globals.inputType = InputType.Menu;
    }

    private void UpdateControls()
    {
        if (controlsGame)
        {
            controlsGame.UpdateText();
        }
        else if (controlsGUI)
        {
            controlsGUI.UpdateText();
        }
    }

    private void ScreenTransition(int screenID)
    {

        currentScreen.screen.gameObject.SetActive(false);
        ResetPosition();

        if (currentScreen.screen.GetComponent<RectTransform>() == null)
        {
            currentScreen.cursor.position = currentScreen.cursorPos;

            currentScreen = screens[screenID];
            currentID = screenID;
            currentScreen.cursorPos = currentScreen.cursor.position;
            currentScreen.screen.gameObject.SetActive(true);
        }
        else
        {
            currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition = currentScreen.cursorPos;

            currentScreen = screens[screenID];
            currentID = screenID;
            currentScreen.cursorPos = currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition;
            currentScreen.screen.gameObject.SetActive(true);
        }
    }

    public void ResetPosition()
    {
        xPos = 0;
        yPos = 0;
        currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition = currentScreen.cursorPos;
    }

    private void HandleVertical(int buttonCount)
    {
        if (currentScreen.menuSettings.verticalEnabled)
        {
            if (Input.GetAxisRaw("Vertical") > 0.5f)
            {
                rapidPrevention = true;
                yPos--;
                if (yPos < 0)
                {
                    yPos = buttonCount - 1;
                    if (currentScreen.cursor.GetComponent<RectTransform>())
                        currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition =
                            new Vector2(currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.x,
                            currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.y + (Vector2.down.y * (currentScreen.menuSettings.verticalOffset * (buttonCount - 1))));
                    else
                        currentScreen.cursor.Translate(Vector2.down * (currentScreen.menuSettings.verticalOffset * (buttonCount - 1)));
                }
                else
                {
                    if (currentScreen.cursor.GetComponent<RectTransform>())
                        currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition =
                           new Vector2(currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.x,
                           currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.y + (Vector2.up.y * currentScreen.menuSettings.verticalOffset));
                    else
                        currentScreen.cursor.Translate(Vector2.up * currentScreen.menuSettings.verticalOffset);
                }

                if (currentScreen.menuSettings.menuIsInventory)
                    MoveInventoryVertical();
            }
            else if (Input.GetAxisRaw("Vertical") < -0.5f)
            {
                rapidPrevention = true;
                yPos++;
                if (yPos > buttonCount - 1)
                {
                    yPos = 0;
                    if (currentScreen.cursor.GetComponent<RectTransform>())
                        currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition =
                            new Vector2(currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.x,
                            currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.y + (Vector2.up.y * (currentScreen.menuSettings.verticalOffset * (buttonCount - 1))));
                    else
                        currentScreen.cursor.Translate(Vector2.up * (currentScreen.menuSettings.verticalOffset * (buttonCount - 1)));
                }
                else
                {
                    if (currentScreen.cursor.GetComponent<RectTransform>())
                        currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition =
                           new Vector2(currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.x,
                           currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.y + (Vector2.down.y * currentScreen.menuSettings.verticalOffset));
                    else
                        currentScreen.cursor.Translate(Vector2.down * currentScreen.menuSettings.verticalOffset);
                }

                if (currentScreen.menuSettings.menuIsInventory)
                    MoveInventoryVertical();
            }
        }
    }

    private void HandleHorizontal(int buttonCount)
    {
        if (currentScreen.menuSettings.horizontalEnabled)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.5f)
            {
                rapidPrevention = true;
                xPos++;
                if (xPos > buttonCount - 1)
                {
                    xPos = 0;
                    if (currentScreen.cursor.GetComponent<RectTransform>())
                        currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition =
                            new Vector2(currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.x + (Vector2.left.x * (currentScreen.menuSettings.horizontalOffset * (buttonCount - 1))),
                            currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.y);
                    else
                        currentScreen.cursor.Translate(Vector2.left * (currentScreen.menuSettings.horizontalOffset * (buttonCount - 1)));
                }
                else
                {
                    if (currentScreen.cursor.GetComponent<RectTransform>())
                        currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition =
                            new Vector2(currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.x + (Vector2.right.x * currentScreen.menuSettings.horizontalOffset),
                            currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.y);
                    else
                        currentScreen.cursor.Translate(Vector2.right * (currentScreen.menuSettings.horizontalOffset));
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.5f)
            {
                rapidPrevention = true;
                xPos--;
                if (xPos < 0)
                {
                    xPos = buttonCount - 1;
                    if (currentScreen.cursor.GetComponent<RectTransform>())
                        currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition =
                            new Vector2(currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.x + (Vector2.right.x * (currentScreen.menuSettings.horizontalOffset * (buttonCount - 1))),
                            currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.y);
                    else
                        currentScreen.cursor.Translate(Vector2.right * (currentScreen.menuSettings.horizontalOffset * (buttonCount - 1)));
                }
                else
                {
                    if (currentScreen.cursor.GetComponent<RectTransform>())
                        currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition =
                            new Vector2(currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.x + (Vector2.left.x * currentScreen.menuSettings.horizontalOffset),
                            currentScreen.cursor.GetComponent<RectTransform>().anchoredPosition.y);
                    else
                        currentScreen.cursor.Translate(Vector2.left * (currentScreen.menuSettings.horizontalOffset));
                }
            }
        }
    }

    private void HandleHorizontalInventory()
    {

        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            MoveInventory(Direction.Right);
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            MoveInventory(Direction.Left);
        }
    }

    private void MoveInventoryVertical()
    {
        foreach (MenuButton btn in currentScreen.buttons)
        {
            if (btn.coordinates.y == yPos)
            {
                if (btn.isActive == false)
                    return;

                invUI.MoveVertical(btn.itemType);
            }
        }
        
    }

    private void MoveInventory(Direction dir)
    {
        rapidPrevention = true;
        foreach (MenuButton btn in currentScreen.buttons)
        {
            if (btn.coordinates.y == yPos)
            {
                if (btn.isActive == false)
                    return;

                xPos = invUI.MoveBar(btn.itemType, dir, (int)xPos);
            }
        }
    }
}

[System.Serializable]
public class MenuScreen
{
    public string name;
    public Menu menuSettings;
    public Transform screen;
    public Transform cursor;
    public List<MenuButton> buttons;

    [HideInInspector]
    public Vector2 cursorPos;
}

[System.Serializable]
public class MenuButton
{
    public bool isActive;
    public Vector2 coordinates;
    public ButtonType buttonType;
    public int screenID;
    public string command;
    public ItemType itemType;
}

