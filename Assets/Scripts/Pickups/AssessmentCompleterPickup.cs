using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssessmentCompleterPickup : Pickable
{
    [SerializeField]
    AudioClip powerupPickupSound;

    private float maxLifetimeSecs = 5f;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, maxLifetimeSecs);
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
        AssignmentController assignmentController = FindObjectOfType<AssignmentController>();
        S_Assignment activeAssmt = assignmentController.GetActiveAssmt();
        for (int i = 0; i < activeAssmt.fields.Count; i++)
        {
            var currentField = activeAssmt.fields[i];
            currentField.currentValue = currentField.targetValue;
            activeAssmt.fields[i] = currentField;
        }
        assignmentController.SetActiveAssmt(activeAssmt);
        FindObjectOfType<AudioSource>().PlayOneShot(powerupPickupSound);

        Destroy(gameObject);
    }

    public void DestroyPickup()
    {
        Destroy(gameObject);
    }
}
