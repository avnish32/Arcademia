using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner_Tut : MonoBehaviour
{
    [SerializeField]
    PickupSpawnPt[] spawnPts;

    [SerializeField]
    float minSpawnDelay, maxSpawnDelay;

    private bool canSpawnPickups = false;
    private List<GameObject> pickupsToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnPickupsRepeatedly()
    {
        while (canSpawnPickups)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay));
            PickupSpawnPt randomSpawnPt = spawnPts[UnityEngine.Random.Range(0, spawnPts.Length)];
            if (randomSpawnPt.CanSpawn())
            {
                randomSpawnPt.SpawnPickup(pickupsToSpawn[UnityEngine.Random.Range(0, pickupsToSpawn.Count)]);
            }
        }
    }

    public GameObject SpawnPickup(GameObject pickupToSpawn)
    {
        PickupSpawnPt randomSpawnPt = spawnPts[UnityEngine.Random.Range(0, spawnPts.Length)];
        while (!randomSpawnPt.CanSpawn())
        {
            randomSpawnPt = spawnPts[UnityEngine.Random.Range(0, spawnPts.Length)];
        }
        var spawnedPickup = randomSpawnPt.SpawnPickup(pickupToSpawn);
        return spawnedPickup;
    }

    public void StartSpawningPickups(List<GameObject> pickupsToSpawn)
    {
        canSpawnPickups = true;
        this.pickupsToSpawn = pickupsToSpawn;
        StartCoroutine(SpawnPickupsRepeatedly());
    }

    public void StopSpawningPickups()
    {
        canSpawnPickups=false;
    }
}
