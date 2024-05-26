using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class AssignmentController_Tut : MonoBehaviour
{
    [SerializeField]
    List<AssmtSlot> assmtSlots;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    AudioClip assmtSubmittedClip;

    [SerializeField]
    GameStateController gameStateController;

    [SerializeField]
    TutorialController tutorialController;

    private List<S_Assignment> assmtQ;
    private int activeAssignmentIndex;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void RemoveAssmt(S_Assignment assignment)
    {
        assmtQ.Remove(assignment);
        activeAssignmentIndex = assmtQ.Count <= 0 ? 0 : --activeAssignmentIndex;
        activeAssignmentIndex = activeAssignmentIndex < 0 ? 0 : activeAssignmentIndex;
        //activeAssignmentIndex = Mathf.Clamp(--activeAssignmentIndex, 0, assmtQ.Count - 1);
        UpdateAssmtUI();
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
