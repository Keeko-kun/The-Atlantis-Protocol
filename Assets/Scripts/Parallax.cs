using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    public Transform cam;
    public BackgroundParallax[] backgrounds;
    [Range(0, 10f)]
    public float damp;

    private Vector3 positionPrevFrame;

	void Start () {
        positionPrevFrame = cam.position;
	}
	
	void Update () {
		foreach (BackgroundParallax bg in backgrounds)
        {
            Transform t = bg.sprite;

            float parallax = (positionPrevFrame.x - cam.position.x) * bg.power * -1;
            Vector3 target = new Vector3(t.position.x + parallax, t.position.y, t.position.z);

            t.position = Vector3.Lerp(t.position, target, damp);
        }

        positionPrevFrame = cam.position;
	}

    public void ResetParallax()
    {
        foreach (BackgroundParallax bg in backgrounds)
        {
            Transform t = bg.sprite;

            t.localPosition = new Vector3(0, 0, t.position.z);
        }
    }
}

[System.Serializable]
public class BackgroundParallax
{
    public Transform sprite;
    [Range(-2.5f, 2.5f)]
    public float power;
}
