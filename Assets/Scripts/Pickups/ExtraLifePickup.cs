using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLifePickup : Pickable
{
    private float maxLifetimeSecs = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, maxLifetimeSecs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Pick()
    {
        PlayerStatsController playerStatsController = FindObjectOfType<PlayerStatsController>();
        playerStatsController.AddLife();
        
        Destroy(gameObject);
    }
}
