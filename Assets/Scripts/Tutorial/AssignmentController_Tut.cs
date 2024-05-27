using ResearchArcade;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentController_Tut : MonoBehaviour
{
    [SerializeField]
    List<AssmtSlot> assmtSlots;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    AudioClip assmtSubmittedClip, assmtMissedClip;

    [SerializeField]
    GameStateController gameStateController;

    [SerializeField]
    TutorialController tutorialController;

    private List<S_Assignment> assmtQ = new List<S_Assignment>();
    private int activeAssignmentIndex;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateAssmtTimers", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (ArcadeInput.Player1.A.Down)
        {
            ChangeActiveAssmt();
        }
    }

    private void SpawnAssmtAndUpdateUI(S_Assignment assmtToSpawn)
    {
        assmtQ.Add(assmtToSpawn);
        assmtSlots[assmtQ.Count - 1].InitSlot(assmtToSpawn);

        if (assmtQ.Count == 1)
        {
            activeAssignmentIndex = 0;
            assmtSlots[assmtQ.Count - 1].SetActiveSlot();
        }
    }

    private int CalculateAssmtScore(S_Assignment assignment)
    {
        int score = 0;
        foreach (var field in assignment.fields)
        {
            score += field.currentValue;
        }
        return score;
    }

    private void UpdateAssmtTimers()
    {
        if (assmtQ.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < assmtQ.Count; i++)
        {
            var currentAssmt = assmtQ[i];
            currentAssmt.timeRemaining -= 1;
            //Debug.Log("i: "+i+" | "+currentAssmt.timeRemaining);
            assmtQ[i] = currentAssmt;
            assmtSlots[i].UpdateUI(currentAssmt);

            if (currentAssmt.timeRemaining <= 0)
            {
                audioSource.PlayOneShot(assmtMissedClip);
                RemoveAssmt(currentAssmt);
                gameStateController.ReduceLife();
            }
        }
    }

    private void RemoveAssmt(S_Assignment assignment)
    {
        assmtQ.Remove(assignment);
        activeAssignmentIndex = assmtQ.Count <= 0 ? 0 : --activeAssignmentIndex;
        activeAssignmentIndex = activeAssignmentIndex < 0 ? 0 : activeAssignmentIndex;
        //activeAssignmentIndex = Mathf.Clamp(--activeAssignmentIndex, 0, assmtQ.Count - 1);
        UpdateAssmtUI();
    }

    private void ChangeActiveAssmt()
    {
        if (++activeAssignmentIndex >= assmtQ.Count)
        {
            activeAssignmentIndex = 0;
        }

        for (int i = 0; i < assmtQ.Count; i++)
        {
            assmtSlots[i].SetInactiveSlot();
        }
        assmtSlots[activeAssignmentIndex].SetActiveSlot();
    }

    public void UpdateAssmtUI()
    {
        int i;
        for (i = 0; i < assmtQ.Count; i++)
        {
            assmtSlots[i].RemoveSlot();
            var currentAssmt = assmtQ[i];
            assmtSlots[i].InitSlot(currentAssmt);

            if (i == activeAssignmentIndex)
            {
                assmtSlots[i].SetActiveSlot();
            }
            else
            {
                assmtSlots[i].SetInactiveSlot();
            }
        }

        for (; i < assmtSlots.Count; i++)
        {
            assmtSlots[i].RemoveSlot();
            assmtSlots[i].SetInactiveSlot();
        }
    }

    public void SpawnFirstAssignment()
    {
        S_Assignment assignment = new S_Assignment();
        assignment.fields = new List<S_AssignmentFieldData>();
        S_AssignmentFieldData field = new S_AssignmentFieldData();
        
        field.field = E_AssignmentFields.PRES_SLIDES;
        field.targetValue = 2;
        assignment.fields.Add(field);

        field.field = E_AssignmentFields.REPORT_PGS;
        field.targetValue = 5;
        assignment.fields.Add(field);

        assignment.timeRemaining = 120;

        SpawnAssmtAndUpdateUI(assignment);
    }

    public void SpawnSecondAssignment()
    {
        S_Assignment assignment = new S_Assignment();
        assignment.fields = new List<S_AssignmentFieldData>();
        S_AssignmentFieldData field = new S_AssignmentFieldData();

        field.field = E_AssignmentFields.CODE_FILES;
        field.targetValue = 2;
        assignment.fields.Add(field);

        field.field = E_AssignmentFields.REPORT_PGS;
        field.targetValue = 3;
        assignment.fields.Add(field);

        assignment.timeRemaining = 120;

        SpawnAssmtAndUpdateUI(assignment);
    }

    public void SpawnThirdAssignment()
    {
        S_Assignment assignment = new S_Assignment();
        assignment.fields = new List<S_AssignmentFieldData>();
        S_AssignmentFieldData field = new S_AssignmentFieldData();

        field.field = E_AssignmentFields.VIDEO_DURATION;
        field.targetValue = 3;
        assignment.fields.Add(field);

        field.field = E_AssignmentFields.REPORT_PGS;
        field.targetValue = 5;
        assignment.fields.Add(field);

        field.field = E_AssignmentFields.CODE_FILES;
        field.targetValue = 3;
        assignment.fields.Add(field);        

        field.field = E_AssignmentFields.PRES_SLIDES;
        field.targetValue = 4;
        assignment.fields.Add(field);

        assignment.timeRemaining = 180;

        SpawnAssmtAndUpdateUI(assignment);
    }

    public void SpawnFourthAssignment()
    {
        S_Assignment assignment = new S_Assignment();
        assignment.fields = new List<S_AssignmentFieldData>();
        S_AssignmentFieldData field = new S_AssignmentFieldData();

        field.field = E_AssignmentFields.CODE_FILES;
        field.targetValue = 2;
        assignment.fields.Add(field);

        field.field = E_AssignmentFields.VIDEO_DURATION;
        field.targetValue = 1;
        assignment.fields.Add(field);

        field.field = E_AssignmentFields.PRES_SLIDES;
        field.targetValue = 7;
        assignment.fields.Add(field);

        field.field = E_AssignmentFields.REPORT_PGS;
        field.targetValue = 10;
        assignment.fields.Add(field);

        assignment.timeRemaining = 90;

        SpawnAssmtAndUpdateUI(assignment);
    }

    public S_Assignment GetActiveAssmt()
    {
        if (assmtQ.Count > 0 && activeAssignmentIndex >= 0)
        {
            return assmtQ[activeAssignmentIndex];
        }
        else
        {
            return new S_Assignment();
        }
    }

    public List<S_Assignment> GetAssmtQ()
    {
        return assmtQ;
    }

    public void SetActiveAssmt(S_Assignment assignment)
    {
        assmtQ[activeAssignmentIndex] = assignment;
        assmtSlots[activeAssignmentIndex].UpdateUI(assignment);
    }

    public bool IsAssmtCompleted(S_Assignment assignment)
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

    public void SubmitCompletedAssmts()
    {
        bool wasAnyAssmtCompleted = false;

        for (int i = assmtQ.Count - 1; i >= 0; i--)
        {
            if (IsAssmtCompleted(assmtQ[i]))
            {
                wasAnyAssmtCompleted = true;
                audioSource.PlayOneShot(assmtSubmittedClip);
                gameStateController.UpdateScore(CalculateAssmtScore(assmtQ[i]));
                RemoveAssmt(assmtQ[i]);
            }
        }

        if (wasAnyAssmtCompleted && tutorialController.OnAssmtSubmitted != null)
        {
            tutorialController.OnAssmtSubmitted.Invoke();
        }
    }

}
