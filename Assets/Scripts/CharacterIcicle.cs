using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIcicle : MonoBehaviour {

    public GameObject icicle;

    private GameObject spawnedIcicle;
    private SpriteRenderer sp;

	void Start () {
        sp = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
        if (GetComponent<PreventInput>().InputProhibited)
            return;

		if (Globals.GetButtonDown("Special") && spawnedIcicle == null)
        {
            GameObject ice = Instantiate(icicle, new Vector3(transform.position.x, transform.position.y, transform.position.z), icicle.transform.rotation);
            if (sp.flipX)
            {
                ice.GetComponent<IcicleAttack>().flipped = true;
            }
            spawnedIcicle = ice;
        }
	}
}
