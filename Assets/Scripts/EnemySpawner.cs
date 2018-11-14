using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;
    public GameObject coinPrefab;

    private GameObject instance;
    private SpriteRenderer sr;

	private void OnEnable () {
        instance = Instantiate(enemyPrefab, transform);
        instance.transform.localPosition = Vector3.zero;
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
	}

    private void OnDisable()
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        foreach(Transform child in children)
        {
            if (child != transform)
                Destroy(child.gameObject);
        }
    }

    public IEnumerator SpawnCoins(Transform enemy, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform);
            coin.transform.position = enemy.position;
            yield return null;
        }
    }

}
