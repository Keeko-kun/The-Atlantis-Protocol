using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using System;
using System.Linq;

public class DisplayDialouge : MonoBehaviour
{

    [Header("Dialouge File")]
    public Dialouge startDialouge;

    [Header("Settings")]
    [Range(0.01f, 0.1f)]
    public float displaySpeed;
    public int maxLetters;

    [Header("Text Objects")]
    public TextMeshPro dialougeText;
    public TextMeshPro actorText;

    [Header("Ink")]
    public TextAsset json;

    private Story ink;
    private bool active = false;
    private bool lineCompleted = false;
    private PreventInput player;
    private FadeGameWorld fade;
    private string display;
    private Dialouge currentDialouge;

    public bool IsActive { get { return active; } }

    public bool IsCutscene { get; set; }

    private void OnEnable()
    {
        fade = GetComponent<FadeGameWorld>();
    }

    public void InitiateDialouge(PreventInput player)
    {
        this.player = player;
        fade.visible = true;

        currentDialouge = startDialouge;

        CheckConditionals();

        ink = new Story(json.text);
        ink.ChoosePathString(currentDialouge.ID);

        NextLine();
    }

    private void CheckConditionals()
    {
        foreach (ChangeDialouge condition in startDialouge.conditions)
        {
            if (Globals.events[condition.eventID])
            {
                currentDialouge = condition.dialouge;
            }
        }
    }

    private void NextLine()
    {
        if (!ink.canContinue)
        {
            StopDialouge();
            return;
        }

        ink.Continue();

        string[] inkLineSplit = ink.currentText.Split(null);

        inkLineSplit = inkLineSplit.Take(inkLineSplit.Count() - 1).ToArray();

        actorText.text = (string)ink.variablesState["actor_name"];
        dialougeText.text = "";

        display = SplitLine(inkLineSplit);

        StartCoroutine(DisplayText(display));

    }

    private void StopDialouge()
    {
        SetEvent();
        GiveItems();

        active = false;
        fade.visible = false;

        if (IsCutscene == false)
            player.InputProhibited = false;

        IsCutscene = false;

        player = null;

    }

    private void GiveItems()
    {
        if (currentDialouge.givesItem)
        {
            CharacterInventory inventory = player.GetComponent<CharacterInventory>();
            inventory.GiveItem(currentDialouge.item, 0);

            if (currentDialouge.setsItem)
            {
                inventory.SetItem(currentDialouge.item);
            }
        }
    }

    public IEnumerator DisplayText(string textToDisplay)
    {
        lineCompleted = false;

        while (dialougeText.text.Length < textToDisplay.Length)
        {
            dialougeText.text += textToDisplay[dialougeText.text.Length];
            yield return new WaitForSecondsRealtime(displaySpeed);
            active = true;
        }

        lineCompleted = true;
    }

    private string SplitLine(string[] inkLineSplit)
    {
        string finalLine = "";
        int count = 0;

        string[] words = inkLineSplit;

        foreach (string word in words)
        {
            count += word.Length + 1;

            if (count > maxLetters)
            {
                count = word.Length + 1;
                finalLine += '\n';
            }

            finalLine += word + ' ';
        }

        return finalLine;
    }

    private void SetEvent()
    {
        foreach (int eventID in currentDialouge.setEventsWhenDone)
        {
            Globals.events[eventID] = true;
        }
    }

    void LateUpdate()
    {
        if (active)
        {
            if (lineCompleted && Globals.GetButtonDown("Action"))
            {
                NextLine();
            }
            else if (!lineCompleted && Globals.GetButtonDown("Action"))
            {
                StopAllCoroutines();
                dialougeText.text = display;
                lineCompleted = true;
            }
        }
    }
}
