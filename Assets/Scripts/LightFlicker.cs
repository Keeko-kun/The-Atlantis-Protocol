using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

    [Range(0, 3)]
    public float maxReduction;
    [Range(0, 3)]
    public float maxIncrease;
    [Range(10, 150)]
    public float flickerStrength;
    [Range(0, 0.3f)]
    public float frequency;

    private Light theLight;
    private float intensity;
    private bool flickering;

	void Start () {

    }

    private void OnEnable()
    {
        theLight = GetComponent<Light>();
        intensity = theLight.intensity;
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        flickering = true;

        while (flickering)
        {
            theLight.intensity = Mathf.Lerp(theLight.intensity, Random.Range(intensity - maxReduction, intensity + maxIncrease), flickerStrength * Time.deltaTime);
            yield return new WaitForSecondsRealtime(frequency);
        }
    }

}
