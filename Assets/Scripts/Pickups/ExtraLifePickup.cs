using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLifePickup : Pickable
{
    [SerializeField]
    AudioClip powerupPickupSound;

    [SerializeField]
    private float maxLifetimeSecs = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterLifetime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(maxLifetimeSecs);
        GetComponent<Animator>().Play("Destroy");
    }

    public override void Pick()
    {
        GameStateController playerStatsController = FindObjectOfType<GameStateController>();
        playerStatsController.AddLife();
        FindObjectOfType<AudioSource>().PlayOneShot(powerupPickupSound);
        
        Destroy(gameObject);
    }

    public void DestroyPickup()
    {
        Destroy(gameObject);
    }
}
