using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private float maxDist = 100f;
    [SerializeField]
    private int objPerFrame = 10;
    [SerializeField]
    private int spawnCount = 100;
    [SerializeField]
    private GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    private Vector3 PointInRange()
    {
        float mult = 1f;
        if (Random.value < 0.5f) mult *= -1f;

        return new Vector3(Random.value * maxDist, Random.value * maxDist, Random.value * maxDist) * mult;
    }

    IEnumerator Spawn()
    {
        int count = 0;
        while(count < spawnCount)
        {
            count++;
            GameObject.Instantiate(prefab, PointInRange(), Random.rotation, transform);
            if(count % objPerFrame == 0) yield return null;
        }
    }
}
