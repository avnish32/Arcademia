using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ResearchArcade;

public class AssignmentController : MonoBehaviour
{
    [SerializeField]
    S_AssignmentFieldData[] assignmentFieldData;

    [SerializeField]
    int minSpawnInterval, maxSpawnInterval;

    [SerializeField]
    int minAssmtLifetime, maxAssmtLifetime;

    [SerializeField]
    int activeAssmtQSize;

    [SerializeField]
    List<AssmtSlot> assmtSlots;

    int minFields=2;
    private Dictionary<E_AssignmentFields, S_AssignmentFieldData> assignmentFieldToFieldDataMap;
    private List<S_Assignment> assmtQ;
    private int activeAssignmentIndex;
    private GameStateController playerStatsController;
    private bool areAssmtsBeingSpawned = true;

    private void Awake()
    {
        playerStatsController = FindObjectOfType<GameStateController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        assignmentFieldToFieldDataMap = new Dictionary<E_AssignmentFields, S_AssignmentFieldData>();
        assmtQ = new List<S_Assignment>();

        foreach (S_AssignmentFieldData fieldDatum in assignmentFieldData)
        {
            assignmentFieldToFieldDataMap[fieldDatum.field] = fieldDatum;
        }
        
        activeAssignmentIndex = 0;
        ChangeActiveAssmt();

        //InvokeRepeating("SpawnAssignment", 3f, 5f);
        SpawnAssmtAndUpdateUI();
        StartCoroutine(SpawnAssignmentRepeatedly());
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

    private IEnumerator SpawnAssignmentRepeatedly()
    {
        areAssmtsBeingSpawned = true;
        while (assmtQ.Count < activeAssmtQSize)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval + 1));
            SpawnAssmtAndUpdateUI();
        }
        areAssmtsBeingSpawned = false;
    }

    private void SpawnAssmtAndUpdateUI()
    {
        S_Assignment spawnedAssmt = SpawnAssignment();
        assmtQ.Add(spawnedAssmt);
        assmtSlots[assmtQ.Count - 1].InitSlot(spawnedAssmt);
    }

    private S_Assignment SpawnAssignment()
    {
        S_Assignment assignment;
        List<S_AssignmentFieldData> assignmentFields = new List<S_AssignmentFieldData> ();
        List<E_AssignmentFields> availableFields = new List<E_AssignmentFields>(assignmentFieldToFieldDataMap.Keys);

        int numFields = UnityEngine.Random.Range(minFields, assignmentFieldToFieldDataMap.Count+1);

        for (int i = 0; i < numFields; i++)
        {
            E_AssignmentFields randomField = availableFields[UnityEngine.Random.Range (0, availableFields.Count)];
            
            S_AssignmentFieldData fieldData = assignmentFieldToFieldDataMap[randomField];
            fieldData.targetValue = UnityEngine.Random.Range(fieldData.minValue, fieldData.maxValue+1);
            fieldData.currentValue = 0;
            assignmentFields.Add(fieldData);

            availableFields.Remove(randomField);
        }

        assignment.fields = assignmentFields;
        assignment.timeRemaining = UnityEngine.Random.Range(minAssmtLifetime, maxAssmtLifetime + 1);

        //DisplayAssignmentData(assignment);
        return assignment;
    }

    private void DisplayAssignmentData(S_Assignment assignment)
    {
        string assignmentDataString = "";
        foreach (var field in assignment.fields)
        {
            assignmentDataString += field.field + ": " + field.currentValue + "/" + field.targetValue + "\n";
        }
        assignmentDataString += "Lifetime in secs: " + assignment.timeRemaining;
        
        Debug.Log(assignmentDataString);
    }

    private void UpdateAssmtTimers()
    {
        if (assmtQ.Count <= 0)
        {
            return;
        }

        for (int i=0; i<assmtQ.Count; i++)
        {
            var currentAssmt = assmtQ[i];
            currentAssmt.timeRemaining -= 1;
            //Debug.Log("i: "+i+" | "+currentAssmt.timeRemaining);
            assmtQ[i] = currentAssmt;
            assmtSlots[i].UpdateUI(currentAssmt);

            if (currentAssmt.timeRemaining <= 0)
            {
                RemoveAssmt(currentAssmt);
                playerStatsController.ReduceLife();
            }
        }
    }

    private void RemoveAssmt(S_Assignment assignment)
    {
        assmtQ.Remove(assignment);
        activeAssignmentIndex = Mathf.Clamp(--activeAssignmentIndex, 0, assmtQ.Count - 1);
        UpdateAssmtUI();
        if (!areAssmtsBeingSpawned)
        {
            StartCoroutine(SpawnAssignmentRepeatedly());
        }
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

    public void UpdateAssmtUI()
    {
        int i;
        for (i = 0; i < assmtQ.Count; i++)
        {
            assmtSlots[i].RemoveSlot();
            var currentAssmt = assmtQ[i];
            assmtSlots[i].InitSlot(currentAssmt);

            if (i==activeAssignmentIndex)
            {
                assmtSlots[i].SetActiveSlot();
            } else
            {
                assmtSlots[i].SetInactiveSlot();
            }
        }

        for (; i<assmtSlots.Count; i++)
        {
            assmtSlots[i].RemoveSlot();
            assmtSlots[i].SetInactiveSlot();
        }
    }

    public S_Assignment GetActiveAssmt()
    {
        return assmtQ[activeAssignmentIndex];
    }

    public void SetActiveAssmt(S_Assignment assignment)
    {
        assmtQ[activeAssignmentIndex] = assignment;
        
        if (IsAssmtCompleted(assignment))
        {
            //change here for submission desk
            RemoveAssmt(assignment);
        } else
        {
            assmtSlots[activeAssignmentIndex].UpdateUI(assignment);
        }
    }

    public void ChangeActiveAssmt()
    {
        if  (++activeAssignmentIndex >= assmtQ.Count)
        {
            activeAssignmentIndex = 0;
        }

        for (int i = 0; i < assmtQ.Count; i++)
        {
            assmtSlots[i].SetInactiveSlot();
        }
        assmtSlots[activeAssignmentIndex].SetActiveSlot();
    }
}
