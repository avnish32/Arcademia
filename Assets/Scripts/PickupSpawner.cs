using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField]
    PickupSpawnPt[] spawnPts;

    [SerializeField]
    float minSpawnDelay, maxSpawnDelay;

    [SerializeField]
    GameObject[] pickups;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPickupsRepeatedly());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnPickupsRepeatedly()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay));
            PickupSpawnPt randomSpawnPt = spawnPts[UnityEngine.Random.Range(0, spawnPts.Length)];
            if (randomSpawnPt.CanSpawn())
            {
                randomSpawnPt.SpawnPickup(pickups[UnityEngine.Random.Range(0, pickups.Length)]);
            }
        }
    }
}
