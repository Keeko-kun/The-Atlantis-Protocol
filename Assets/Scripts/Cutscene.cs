using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class Cutscene : MonoBehaviour {

    [Header("Actors & Objects")]
    public List<CutsceneObject> components;

    [Header("Ink")]
    public TextAsset json;
    public Dialouge cutscene;

    [Header("Important Objects")]
    public FadeUI darkness;

    private PreventInput player;
    private Story ink;

	void OnEnable () {

	}

    public IEnumerator InitiateCutscene(PreventInput player)
    {
        ink = new Story(json.text);
        ink.ChoosePathString(cutscene.ID);
        this.player = player;

        yield return StartCoroutine(NextLine());
    }

    private IEnumerator NextLine()
    {
        while (ink.canContinue)
        {
            ink.Continue();

            string[] inkLineSplit = ink.currentText.Split(null);

            inkLineSplit = inkLineSplit.Take(inkLineSplit.Count() - 1).ToArray();

            yield return StartCoroutine(RunCommand(inkLineSplit));
        }
    }

    public IEnumerator RunCommand(string[] words)
    {
        switch (words[0])
        {
            case "!move":
                yield return StartCoroutine(Move(words[1], words[2]));
                break;
            case "!flipx":
                yield return StartCoroutine(FlipX(words[1]));
                break;
            case "!speak":
                yield return StartCoroutine(Speak(words[1]));
                break;
            case "!pause":
                yield return StartCoroutine(Pause(words[1]));
                break;
            case "!save":
                yield return StartCoroutine(Save());
                break;
        }
    }

    private IEnumerator FlipX(string actorName)
    {
        SpriteRenderer sr = FindComponent(actorName, ComponentType.SpriteRenderer).GetComponent<SpriteRenderer>();
        sr.flipX = !sr.flipX;
        yield return null;
    }

    private IEnumerator Speak(string actorName)
    {
        DisplayDialouge dialouge = FindComponent(actorName, ComponentType.DisplayDialouge).GetComponent<DisplayDialouge>();
        dialouge.InitiateDialouge(player);
        dialouge.IsCutscene = true;

        yield return new WaitForSecondsRealtime(dialouge.displaySpeed + 0.01f);

        yield return new WaitUntil(() => dialouge.IsActive == false);
    }

    private IEnumerator Move(string actorName, string distanceString)
    {
        int distance = int.Parse(distanceString);
        MoveCutscene move = FindComponent(actorName, ComponentType.MoveCutscene).GetComponent<MoveCutscene>();
        yield return StartCoroutine(move.Move(distance));
    }

    private IEnumerator Pause(string timeString)
    {
        float time = float.Parse(timeString, CultureInfo.InvariantCulture);
        yield return new WaitForSecondsRealtime(time);
    }

    private IEnumerator Save()
    {
        if (GetComponentInParent<SaveProgress>())
        {
           yield return StartCoroutine(GetComponentInParent<SaveProgress>().SaveGame(player.gameObject));
        }
    }

    private Transform FindComponent(string objectName, ComponentType type)
    {
        foreach (CutsceneObject co in components)
        {
            if (co.name == objectName && co.type == type)
                return co.obj;
        }

        return null;
    }
}


