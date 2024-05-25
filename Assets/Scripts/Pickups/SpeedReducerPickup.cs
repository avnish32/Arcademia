using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedReducerPickup : Pickable
{
    [SerializeField]
    AudioClip enemyPickupSound;

    private float maxLifetimeSecs = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterLifetime());
        //Destroy(gameObject, maxLifetimeSecs);
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
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        playerMovement.OnSpeedReducerPickup();

        FindObjectOfType<AudioSource>().PlayOneShot(enemyPickupSound);

        Destroy(gameObject);
    }

    public void DestroyPickup()
    {
        Destroy(gameObject);
    }
}
