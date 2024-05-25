using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject creditsPanel, htpPanel, titlePanel, infoPanel;
    
    private GameObject activePanel;

    // Start is called before the first frame update
    void Start()
    {
        titlePanel.SetActive(true);
        infoPanel.SetActive(true);
        activePanel = infoPanel;

        creditsPanel.SetActive(false);
        htpPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCreditsButtonClicked()
    {
        if (activePanel != null)
        {
            activePanel.SetActive(false);
        }
        titlePanel.SetActive(true);
        creditsPanel.SetActive(true);
        activePanel = creditsPanel;
    }

    public void OnHTPButtonClicked()
    {
        if (activePanel != null)
        {
            activePanel.SetActive(false);
        }
        titlePanel.SetActive(false);
        htpPanel.SetActive(true);
        activePanel = htpPanel;
    }
}
