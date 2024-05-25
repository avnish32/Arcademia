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
            int nextActiveButton = Mathf.Clamp(currentActiveButton-1, 0, buttons.Length-1);
            while (nextActiveButton > -1 && !buttons[nextActiveButton].gameObject.activeInHierarchy)
            {
                nextActiveButton = Mathf.Clamp(nextActiveButton - 1, 0, buttons.Length - 1);
            }
            currentActiveButton = nextActiveButton>-1?nextActiveButton:currentActiveButton;
            UpdateHighlightedButton();
        } else if (ArcadeInput.Player1.JoyDown.Down)
        {
            int nextActiveButton = Mathf.Clamp(currentActiveButton + 1, 0, buttons.Length - 1);
            while (nextActiveButton < buttons.Length && !buttons[nextActiveButton].gameObject.activeInHierarchy)
            {
                nextActiveButton = Mathf.Clamp(nextActiveButton + 1, 0, buttons.Length - 1);
            }
            currentActiveButton = nextActiveButton < buttons.Length ? nextActiveButton : currentActiveButton;
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

    public void SetActiveButton(int buttonIndex)
    {
        currentActiveButton = buttonIndex;
        UpdateHighlightedButton();
    }
}
