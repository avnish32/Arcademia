using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmissionPt : MonoBehaviour
{
    [SerializeField]
    SubmissionPanel submissionPanel;

    [SerializeField]
    private TutorialController tutorialController;

    private AssignmentController assignmentController;
    private List<S_Assignment> assmtQ;

    // Start is called before the first frame update
    void Start()
    {
        submissionPanel.gameObject.SetActive(false);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (assignmentController == null)
        {
            assignmentController = FindObjectOfType<AssignmentController>();
        }


        assignmentController.SubmitCompletedAssmts();

        if (tutorialController != null && tutorialController.OnAssmtSubmitted != null)
        {
            tutorialController.OnAssmtSubmitted.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (assignmentController == null)
        {
            assignmentController = FindObjectOfType<AssignmentController>();
        }

        if (gameStateController == null)
        {
            gameStateController = FindObjectOfType<GameStateController>();
        }

        gameStateController.PauseGame();
        BuildSubmissionPanel();*/
    }

    private void BuildSubmissionPanel()
    {
        assmtQ = assignmentController.GetAssmtQ();
        int i;
        for (i = 0; i <assmtQ.Count; i++)
        {
            string buttonText = string.Format("Assessment {0}", i + 1);
            if (IsAssmtCompleted(assmtQ[i]))
            {
                buttonText += " - Completed";
            } else
            {
                buttonText += " - Incomplete";
            }
            submissionPanel.assmtButtons[i].SetButtonText(buttonText);
            submissionPanel.assmtButtons[i].gameObject.SetActive(true);
        }

        for (; i<submissionPanel.assmtButtons.Length; i++)
        {
            submissionPanel.assmtButtons[i].gameObject.SetActive(false);
        }

        submissionPanel.backButton.gameObject.SetActive(true);
        submissionPanel.GetComponent<UIInteraction>().SetActiveButton(submissionPanel.assmtButtons.Length);
        submissionPanel.gameObject.SetActive(true);

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

    /*public void OnAssmtSubmitButtonClicked(int submittedAssmtIndex)
    {
        assignmentController.SubmitCompletedAssmts(assmtQ[submittedAssmtIndex]);
        BuildSubmissionPanel();
    }*/
}
