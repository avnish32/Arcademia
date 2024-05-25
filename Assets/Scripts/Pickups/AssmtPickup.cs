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

    [SerializeField]
    AudioClip assmtPickupSound;

    private int pickupValue; 
    private float maxLifetimeSecs=10f;

    // Start is called before the first frame update
    void Start()
    {
        pickupValue = UnityEngine.Random.Range(minPickupValue, maxPickupValue+1);
        pickupValueText.text = string.Format("x{0}", pickupValue);

        maxLifetimeSecs = maxLifetimeSecs * (1f / pickupValue);

        //Destroy(gameObject, maxLifetimeSecs);
        StartCoroutine(DestroyAfterLifetime());
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(maxLifetimeSecs);
        GetComponent<Animator>().Play("Destroy");
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
                currentField.currentValue = Mathf.Clamp(currentField.currentValue, 0, currentField.targetValue);
                activeAssmt.fields[i] = currentField;
                break;
            }
        }

        assignmentController.SetActiveAssmt(activeAssmt);
        FindObjectOfType<AudioSource>().PlayOneShot(assmtPickupSound);
        Destroy(gameObject);
    }

    public void DestroyPickup()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
