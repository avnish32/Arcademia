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
    float assmtPickupProb, powerupProb, enemiesProb;

    [SerializeField]
    GameObject[] assmtPickups, powerups, enemies;

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
                float prob = UnityEngine.Random.Range(0f, 1f);
                if (prob <= assmtPickupProb)
                {
                    randomSpawnPt.SpawnPickup(assmtPickups[UnityEngine.Random.Range(0, assmtPickups.Length)]);
                } else if (prob <= assmtPickupProb+powerupProb)
                {
                    randomSpawnPt.SpawnPickup(powerups[UnityEngine.Random.Range(0, powerups.Length)]);
                } else
                {
                    randomSpawnPt.SpawnPickup(enemies[UnityEngine.Random.Range(0, enemies.Length)]);
                }   
            }
        }
    }
}
