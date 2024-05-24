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
    GameObject fieldsPanel, fieldTextSlot;

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = true;
        this.gameObject.SetActive(true);
        GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableSlot(S_Assignment assignment)
    {
        GetComponent<Image>().enabled = true;
        //enable other panels here
        UpdateUI(assignment);
    }

    public void UpdateUI(S_Assignment assignment)
    {
        timerText.text = "0:" + assignment.timeRemaining;
    }
}
