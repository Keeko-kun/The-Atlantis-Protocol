using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AreaIntroduction : MonoBehaviour {

    public TextMeshProUGUI tmp;
    private Animator anim;

	void Awake () {
        anim = GetComponent<Animator>();
	}

    public void Introduce(string message)
    {
        if (message.Contains("|"))
        {
            tmp.text = "";
            string[] split = message.Split('|');
            foreach (string s in split)
            {
                tmp.text += s + '\n';
            }
        }
        else
        {
            tmp.text = message;
        }

        StopAllCoroutines();
        StartCoroutine(RunIntroduction());
    }
	
    private IEnumerator RunIntroduction()
    {
        anim.SetBool("Show", true);
        yield return null;
        anim.SetBool("Show", false);
    }
}
