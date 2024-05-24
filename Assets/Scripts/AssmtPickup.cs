using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AssmtPickup : Pickable
{
    [SerializeField]
    E_AssignmentFields pickupField;
    [SerializeField]
    int minPickupValue, maxPickupValue;
    [SerializeField]
    TextMeshProUGUI pickupValueText;

    private int pickupValue; 
    private float maxLifetimeSecs=100f;

    // Start is called before the first frame update
    void Start()
    {
        pickupValue = UnityEngine.Random.Range(minPickupValue, maxPickupValue+1);
        pickupValueText.text = string.Format("x{0}", pickupValue);

        maxLifetimeSecs = maxLifetimeSecs * (1f / pickupValue);

        Destroy(gameObject, maxLifetimeSecs);
    }

    public override void Pick()
    {
        AssignmentController assignmentController = FindObjectOfType<AssignmentController>();
        var activeAssmt = assignmentController.GetActiveAssmt();

        for (int i = 0; i<activeAssmt.fields.Count; i++)
        {
            var currentField = activeAssmt.fields[i];
            if (currentField.field == pickupField)
            {
                currentField.currentValue += pickupValue;
                activeAssmt.fields[i] = currentField;
                break;
            }
        }

        assignmentController.SetActiveAssmt(activeAssmt);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
