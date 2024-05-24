using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssmtSlot : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timerText;

    [SerializeField]
    GameObject fieldsPanel, timerPanel, fieldTextSlot, activeSlotIndicator;

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = true;
        this.gameObject.SetActive(true);
        
        GetComponent<Image>().enabled = false;
        fieldsPanel.SetActive(false);
        timerPanel.SetActive(false);
        activeSlotIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitSlot(S_Assignment assignment)
    {
        GetComponent<Image>().enabled = true;
        //enable other panels here
        fieldsPanel.SetActive(true);
        timerPanel.SetActive(true);

        foreach (var field in assignment.fields)
        {
            var newFieldTextSlot = Instantiate(fieldTextSlot);
            newFieldTextSlot.transform.SetParent(fieldsPanel.transform);
        }

        UpdateUI(assignment);
    }

    public void UpdateUI(S_Assignment assignment)
    {
        timerText.text = "0:" + assignment.timeRemaining;

        var fieldTextSlots = fieldsPanel.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < fieldTextSlots.Length; i++)
        {
            var assmtField = assignment.fields[i];
            fieldTextSlots[i].text = string.Format("{0} {1}/{2}", assmtField.field, assmtField.currentValue, assmtField.targetValue);
        }
    }

    public void SetActiveSlot()
    {
        activeSlotIndicator.SetActive(true);
    }

    public void SetInactiveSlot()
    {
        activeSlotIndicator.SetActive(false);
    }
}
