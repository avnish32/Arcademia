using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AssmtPickup_Tut : Pickable
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
    private float maxLifetimeSecs = 10f;
    private TutorialController tutorialController;

    // Start is called before the first frame update
    void Start()
    {
        pickupValue = UnityEngine.Random.Range(minPickupValue, maxPickupValue + 1);
        pickupValueText.text = string.Format("x{0}", pickupValue);

        maxLifetimeSecs = maxLifetimeSecs * (1f / pickupValue);

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

    private void DestroyByUser()
    {
        if (tutorialController.OnPickupCollected != null)
        {
            tutorialController.OnPickupCollected.Invoke();
        }

        FindObjectOfType<AudioSource>().PlayOneShot(assmtPickupSound);
        Destroy(gameObject);
    }

    public override void Pick()
    {
        AssignmentController_Tut assignmentController = FindObjectOfType<AssignmentController_Tut>();
        var activeAssmt = assignmentController.GetActiveAssmt();

        if (activeAssmt.fields == null || activeAssmt.fields.Count <= 0)
        {
            DestroyByUser();
            return;
        }

        for (int i = 0; i < activeAssmt.fields.Count; i++)
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
        DestroyByUser();
    }

    public void DestroyPickup()
    {
        if (tutorialController.OnPickupNotCollected != null)
        {
            tutorialController.OnPickupNotCollected.Invoke();
        }
        Destroy(gameObject);
    }
}
