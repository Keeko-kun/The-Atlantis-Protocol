using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class SetFileSelect : MonoBehaviour {

    public bool newGameScreen;

    private List<Transform> files;

	void Start () {
        files = GetComponentsInChildren<Transform>(true).ToList();

        List<Transform> filesExclude = new List<Transform>();

        foreach (Transform file in files)
        {
            if (file.tag == Tags.SaveFile.ToString())
                filesExclude.Add(file);
        }

        files = filesExclude;
        SetFiles();

    }

    private void SetFiles()
    {

        for (int i = 0; i < files.Count; i++)
        {
            TextMeshPro[] texts = files[i].GetComponentsInChildren<TextMeshPro>(true);

            if (PlayerPrefs.HasKey("tfpSave_" + i.ToString()) == false || PlayerPrefsX.GetBool("tfpSave_" + i.ToString()) == false)
            {
                texts[0].text = "New File";
                texts[1].text = "";

                if (newGameScreen)
                    texts[2].gameObject.SetActive(false);

                continue;
            }
            else
            {
                texts[0].text = "File " + (i + 1).ToString() + ':';
                texts[1].text = PlayerPrefs.GetString("tfpLocation_" + i.ToString());

                if (newGameScreen)
                    texts[2].gameObject.SetActive(true);
            }

        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
            SetFiles();
        }
    }
#endif
}
