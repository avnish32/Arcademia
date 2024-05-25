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
    AssmtFieldSlot[] fieldSlots;

    [SerializeField]
    S_FieldIconData[] fieldIconData;

    [SerializeField]
    AudioClip assmtCompletedClip;

    private Dictionary<E_AssignmentFields, Sprite> fieldToIconMap;
    private AudioSource audioSource;

    private void Awake()
    {
        //Debug.Log("Inside awake of Assmt slot.");
        fieldToIconMap = new Dictionary<E_AssignmentFields, Sprite>();
        foreach (var fieldIconDatum in fieldIconData)
        {
            fieldToIconMap[fieldIconDatum.field] = fieldIconDatum.fieldIcon;
        }

        this.enabled = true;
        this.gameObject.SetActive(true);

        GetComponent<Image>().enabled = false;
        fieldsPanel.SetActive(false);
        timerPanel.SetActive(false);
        activeSlotIndicator.SetActive(false);

        foreach (var fieldSlot in fieldSlots)
        {
            fieldSlot.gameObject.SetActive(false);
        }

        audioSource = FindObjectOfType<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Inside start of Assmt slot.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool IsAssmtCompleted(S_Assignment assignment)
    {
        foreach (var field in assignment.fields)
        {
            if (field.currentValue < field.targetValue)
            {
                return false;
            }
        }
        return true;
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
            fieldSlots[i].gameObject.SetActive(true);
        }
        UpdateAssmtSlotBGColor(assignment);

        UpdateUI(assignment);
    }

    public void RemoveSlot()
    {
        var fieldSlots = fieldsPanel.GetComponentsInChildren<AssmtFieldSlot>();
        foreach (var fieldSlot in fieldSlots)
        {
            fieldSlot.gameObject.SetActive(false);
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
        UpdateAssmtSlotBGColor(assignment);

        //var fieldTextSlots = fieldsPanel.GetComponentsInChildren<TextMeshProUGUI>();
        //Debug.Log("Updating slot UI, field text slots count: " + fieldTextSlots.Length+" | assignment field count: "+ assignment.fields.Count);

        for (int i = 0; i < assignment.fields.Count; i++)
        {
            var assmtField = assignment.fields[i];

            fieldSlots[i].fieldIcon.sprite = fieldToIconMap[assmtField.field];

            fieldSlots[i].fieldValText.text = string.Format("{0}/{1}", assmtField.currentValue, assmtField.targetValue);
            Color fieldTextColor = new Color32(67, 0, 0, 255);

            if (assmtField.currentValue >= assmtField.targetValue)
            {
                fieldTextColor = new Color32(0, 67, 0, 255);
            }
            else
            {
                float ratio = (float)assmtField.currentValue / (float)assmtField.targetValue;
                //Debug.Log("Ratio: " + ratio);
                if (ratio > 0.5)
                {
                    fieldTextColor = new Color32(67, 67, 0, 255);
                }
                else if (ratio > 0)
                {
                    fieldTextColor = new Color32(67, 38, 0, 255);

                }
            }

            fieldSlots[i].fieldValText.color = fieldTextColor;
        }
    }

    private void UpdateAssmtSlotBGColor(S_Assignment assignment)
    {
        if (IsAssmtCompleted(assignment))
        {
            if (audioSource == null)
            {
                audioSource = FindObjectOfType<AudioSource>();
            }
            audioSource.PlayOneShot(assmtCompletedClip);
            Debug.Log("assmt slot changed itself to green.");
            GetComponent<Image>().color = new Color32(122, 255, 93, 255); //green
        }
        else
        {
            if (assignment.timeRemaining < 5)
            {
                GetComponent<Image>().color = new Color32(255, 120, 94, 255); //red
            }
            else if (assignment.timeRemaining < 10)
            {
                GetComponent<Image>().color = new Color32(255, 168, 94, 255); //orange
            } else
            {
                GetComponent<Image>().color = new Color32(235, 255, 93, 255); //yrllow
            }
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
