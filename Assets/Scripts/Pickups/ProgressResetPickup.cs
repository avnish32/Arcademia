using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressResetPickup : Pickable
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
        AssignmentController assignmentController = FindObjectOfType<AssignmentController>();
        S_Assignment activeAssmt = assignmentController.GetActiveAssmt();
        for (int i = 0; i<activeAssmt.fields.Count; i++)
        {
            var currentField = activeAssmt.fields[i];
            currentField.currentValue = 0;
            activeAssmt.fields[i] = currentField;
        }
        assignmentController.SetActiveAssmt(activeAssmt);

        Destroy(gameObject);
    }
}
