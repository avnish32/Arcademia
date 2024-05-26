using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressResetPickup_Tut : Pickable
{
    [SerializeField]
    AudioClip enemyPickupSound;

    [SerializeField]
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
        AssignmentController_Tut assignmentController = FindObjectOfType<AssignmentController_Tut>();
        S_Assignment activeAssmt = assignmentController.GetActiveAssmt();

        if (activeAssmt.fields == null || activeAssmt.fields.Count <= 0)
        {
            FindObjectOfType<AudioSource>().PlayOneShot(enemyPickupSound);
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i<activeAssmt.fields.Count; i++)
        {
            var currentField = activeAssmt.fields[i];
            currentField.currentValue = 0;
            activeAssmt.fields[i] = currentField;
        }
        assignmentController.SetActiveAssmt(activeAssmt);

        FindObjectOfType<AudioSource>().PlayOneShot(enemyPickupSound);
        Destroy(gameObject);
    }

    public void DestroyPickup()
    {
        Destroy(gameObject);
    }
}
