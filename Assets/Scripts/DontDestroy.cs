using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{

    public static DontDestroy instance;

    public InputType initialInputType;

    private void Start()
    {
        DoGlobals();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void DoGlobals()
    {
        Globals.InitiateInputs();
        Globals.inputType = initialInputType;
    }

    public static GameObject GetInstance()
    {
        return instance.gameObject;
    }
}
