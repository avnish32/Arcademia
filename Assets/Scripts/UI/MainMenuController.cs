using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject creditsPanel;
    
    private GameObject activePanel;

    // Start is called before the first frame update
    void Start()
    {
        creditsPanel.SetActive(false);
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
        creditsPanel.SetActive(true);
        activePanel = creditsPanel;
    }
}
