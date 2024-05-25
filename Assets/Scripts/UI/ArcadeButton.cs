using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArcadeButton : MonoBehaviour
{
    [SerializeField]
    GameObject buttonHighlighter;

    [SerializeField]
    TextMeshProUGUI buttonText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick()
    {
        GetComponent<Button>().onClick.Invoke();
    }

    public void Highlight()
    {
        buttonHighlighter.SetActive(true);
    }

    public void RemoveHighlight()
    {
        buttonHighlighter.SetActive(false);
    }

    public void SetButtonText(string buttonText)
    {
        this.buttonText.text = buttonText;
    }
}
