using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static class Globals
{
    public static bool[] events = new bool[500];

    public static InputType inputType;

    private static Dictionary<string, string[]> commands;

    public static void InitiateInputs()
    {
        commands = new Dictionary<string, string[]>();

        if (PlayerPrefs.HasKey("tfpInputs") == false)
        {
            SetInputsStandard();
        }
        else
        {
            string[] inputs = PlayerPrefsX.GetStringArray("tfpInputs");
            foreach (string input in inputs)
            {
                string[] split = input.Split('|');
                commands[split[0]] = new string[2] {split[1], split[2] };
            }
        }

    }

    private static void SetInputsStandard()
    {
        commands["Jump"] = new string[2] { KeyCode.Space.ToString(), GamePad.AButton.ToString() };
        commands["Attack"] = new string[2] { KeyCode.Z.ToString(), GamePad.XButton.ToString() };
        commands["Special"] = new string[2] { KeyCode.X.ToString(), GamePad.BButton.ToString() };
        commands["Crouch"] = new string[2] { KeyCode.DownArrow.ToString(), "Down" };
        commands["Item"] = new string[2] { KeyCode.C.ToString(), GamePad.YButton.ToString() };
        commands["Menu"] = new string[2] { KeyCode.S.ToString(), GamePad.StartButton.ToString() };
        commands["Map"] = new string[2] { KeyCode.LeftShift.ToString(), GamePad.LeftBumper.ToString() };
        commands["Action"] = new string[2] { KeyCode.A.ToString(), GamePad.RightBumper.ToString() };

        SaveInputs();
    }

    private static void SaveInputs()
    {
        List<string> inputs = new List<string>();

        foreach (string key in commands.Keys)
        {
            string input = key + '|' + commands[key][0] + '|' + commands[key][1];
            inputs.Add(input);
        }

        PlayerPrefsX.SetStringArray("tfpInputs", inputs.ToArray());
    }

    public static void ChangeInputs(string command, KeyCode kcode)
    {
        foreach(KeyValuePair<string, string[]> entry in commands)
        {
            if (entry.Value[0] == kcode.ToString())
            {
                commands[entry.Key][0] = commands[command][0];
                break;
            }
        }

        commands[command][0] = kcode.ToString();

        SaveInputs();
    }

    public static void ChangeInputs(string command, GamePad gcode)
    {
        foreach (KeyValuePair<string, string[]> entry in commands)
        {
            if (entry.Value[1] == gcode.ToString())
            {
                commands[entry.Key][1] = commands[command][1];
                break;
            }
        }

        commands[command][1] = gcode.ToString();

        SaveInputs();
    }

    public static string[] GetButtons(string key)
    {
        return commands[key];
    }

    public static bool GetButtonDown(string command)
    {
        if (!commands.ContainsKey(command))
        {
            Debug.LogError("No command named: " + command);
        }

        KeyCode key = (KeyCode) Enum.Parse(typeof(KeyCode), commands[command][0], true);

        string button = commands[command][1].ToString();

        return Input.GetKeyDown(key) || Input.GetButtonDown(button);
    }
}