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

    [SerializeField]
    TextMeshProUGUI[] fieldTextSlots;

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = true;
        this.gameObject.SetActive(true);
        
        GetComponent<Image>().enabled = false;
        fieldsPanel.SetActive(false);
        timerPanel.SetActive(false);
        activeSlotIndicator.SetActive(false);

        foreach (var fieldTextSlot in fieldTextSlots)
        {
            fieldTextSlot.enabled = false;
        }
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

        //Debug.Log("no. of fields before making field text slots: " + assignment.fields.Count);
        for (int i = 0; i<assignment.fields.Count; i++)
        {
            fieldTextSlots[i].enabled = true;
        }

        UpdateUI(assignment);
    }

    public void RemoveSlot()
    {
        var fieldTextSlots = fieldsPanel.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var fieldTextSlot in fieldTextSlots)
        {
            fieldTextSlot.enabled = false;
        }
        fieldsPanel.SetActive(false);
        timerPanel.SetActive(false);
        GetComponent<Image>().enabled = false;

        //Debug.Log("After destroying, tmpro under field panel: " + fieldsPanel.GetComponentsInChildren<TextMeshProUGUI>().Length);
    }

    public void UpdateUI(S_Assignment assignment)
    {
        int minsLeft = assignment.timeRemaining / 60;
        int secsLeft = assignment.timeRemaining % 60;
        
        timerText.text = string.Format("{0}:{1}", minsLeft.ToString("00"), secsLeft.ToString("00"));
        
        //var fieldTextSlots = fieldsPanel.GetComponentsInChildren<TextMeshProUGUI>();
        //Debug.Log("Updating slot UI, field text slots count: " + fieldTextSlots.Length+" | assignment field count: "+ assignment.fields.Count);

        for (int i = 0; i < assignment.fields.Count; i++)
        {
            var assmtField = assignment.fields[i];
            fieldTextSlots[i].text = string.Format("{0} {1}/{2}", assmtField.field, assmtField.currentValue, assmtField.targetValue);
            Color fieldTextColor = Color.red;

            if (assmtField.currentValue >= assmtField.targetValue)
            {
                fieldTextColor = Color.green;
            } else
            {
                float ratio = (float)assmtField.currentValue / (float)assmtField.targetValue;
                //Debug.Log("Ratio: " + ratio);
                if (ratio > 0.5)
                {
                    fieldTextColor = Color.yellow;
                } else if (ratio > 0)
                {
                    fieldTextColor = new Color32(255, 90, 0, 255);
                    
                }
            }
            
            fieldTextSlots[i].color = fieldTextColor;
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
