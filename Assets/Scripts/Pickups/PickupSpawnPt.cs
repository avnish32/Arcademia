using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawnPt : MonoBehaviour
{
    private GameObject pickupAtThisPt;
    private PlayerMovement player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnPickup(GameObject pickup)
    {
        //Debug.Log("Pickup spawned.");
        pickupAtThisPt = Instantiate(pickup, transform.position, Quaternion.identity);
        return pickupAtThisPt;
    }

    public bool CanSpawn()
    {
        float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);
        return pickupAtThisPt == null && distanceFromPlayer > 1f;
    }

}
