using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResearchArcade;

public class UIInteraction : MonoBehaviour
{
    [SerializeField]
    ArcadeButton[] buttons;

    private int currentActiveButton = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHighlightedButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (ArcadeInput.Player1.JoyUp.Down)
        {
            currentActiveButton = Mathf.Clamp(--currentActiveButton, 0, buttons.Length-1);
            UpdateHighlightedButton();
        } else if (ArcadeInput.Player1.JoyDown.Down)
        {
            currentActiveButton = Mathf.Clamp(++currentActiveButton, 0, buttons.Length - 1);
            UpdateHighlightedButton();
        } else if (ArcadeInput.Player1.Start.Down)
        {
            buttons[currentActiveButton].OnButtonClick();
        }
    }

    private void UpdateHighlightedButton()
    {
        buttons[currentActiveButton].Highlight();

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i==currentActiveButton)
            {
                continue;
            }
            buttons[i].RemoveHighlight();
        }
    }
}
