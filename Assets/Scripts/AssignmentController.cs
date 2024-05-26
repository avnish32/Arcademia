using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ResearchArcade;

public class AssignmentController : MonoBehaviour
{
    [SerializeField]
    S_AssignmentWaveData[] assignmentWaveData;

    [SerializeField]
    S_AssignmentFieldData[] assignmentFieldData;

    [SerializeField]
    int minSpawnInterval, maxSpawnInterval;

    [SerializeField]
    int minAssmtSecsPerTarget, maxAssmtSecsPerTarget;

    [SerializeField]
    int activeAssmtQSize;

    [SerializeField]
    List<AssmtSlot> assmtSlots;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    AudioClip assmtSubmittedClip, waveEndClip, winClip, assmtMissedClip;

    int minFields=2;
    private Dictionary<E_AssignmentFields, S_AssignmentFieldData> assignmentFieldToFieldDataMap;
    private Dictionary<int, S_AssignmentWaveData> assmtWaveToWaveDataMap;
    private List<S_Assignment> assmtQ;
    private int activeAssignmentIndex;
    private GameStateController gameStateController;
    private bool areAssmtsBeingSpawned = true;
    private int currentWave = -1, assmtsSpawnedInCurrentWave=0;

    private void Awake()
    {
        //Debug.Log("Inside awake of Assmt controller.");
        gameStateController = FindObjectOfType<GameStateController>();
        audioSource = FindObjectOfType<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Inside start of Assmt controller.");
        assmtQ = new List<S_Assignment>();

        assignmentFieldToFieldDataMap = new Dictionary<E_AssignmentFields, S_AssignmentFieldData>();
        foreach (S_AssignmentFieldData fieldDatum in assignmentFieldData)
        {
            assignmentFieldToFieldDataMap[fieldDatum.field] = fieldDatum;
        }

        assmtWaveToWaveDataMap = new Dictionary<int, S_AssignmentWaveData>();
        for (int i = 0; i<assignmentWaveData.Length; i++)
        {
            assmtWaveToWaveDataMap[i] = assignmentWaveData[i];
        }
        
        activeAssignmentIndex = 0;
        ChangeActiveAssmt();

        //InvokeRepeating("SpawnAssignment", 3f, 5f);
        StartNewWave();
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

    private void StartNewWave()
    {
        currentWave++;
        assmtsSpawnedInCurrentWave = 0;
        minAssmtSecsPerTarget = (int)assmtWaveToWaveDataMap[currentWave].minAssmtSecsPerTarget;
        maxAssmtSecsPerTarget = (int)assmtWaveToWaveDataMap[currentWave].maxAssmtSecsPerTarget;

        SpawnAssmtAndUpdateUI();
        UpdateAssmtUI();
        StartCoroutine(SpawnAssignmentRepeatedly());
    }
    private IEnumerator SpawnAssignmentRepeatedly()
    {
        areAssmtsBeingSpawned = true;
        while (assmtQ.Count < activeAssmtQSize && assmtsSpawnedInCurrentWave < assmtWaveToWaveDataMap[currentWave].numAssmts)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval + 1));
            if (gameStateController.isGamePaused)
            {
                continue;
            }
            SpawnAssmtAndUpdateUI();
        }
        areAssmtsBeingSpawned = false;
    }

    private void SpawnAssmtAndUpdateUI()
    {
        S_Assignment spawnedAssmt = SpawnAssignment();
        assmtQ.Add(spawnedAssmt);
        assmtSlots[assmtQ.Count - 1].InitSlot(spawnedAssmt);

        if (assmtQ.Count == 1)
        {
            activeAssignmentIndex = 0;
            assmtSlots[assmtQ.Count - 1].SetActiveSlot();
        }
    }

    private S_Assignment SpawnAssignment()
    {
        S_Assignment assignment;
        List<S_AssignmentFieldData> assignmentFields = new List<S_AssignmentFieldData> ();
        List<E_AssignmentFields> availableFields = new List<E_AssignmentFields>(assignmentFieldToFieldDataMap.Keys);

        int numFields = assignmentFieldToFieldDataMap.Count;
        if (currentWave < assmtWaveToWaveDataMap.Count-1)
        {
            numFields = UnityEngine.Random.Range(minFields, assignmentFieldToFieldDataMap.Count + 1);
        }
         
        int totalTarget = 0;

        for (int i = 0; i < numFields; i++)
        {
            E_AssignmentFields randomField = availableFields[UnityEngine.Random.Range (0, availableFields.Count)];
            
            S_AssignmentFieldData fieldData = assignmentFieldToFieldDataMap[randomField];
            fieldData.targetValue = (int)Mathf.Ceil(UnityEngine.Random.Range(fieldData.minValue, fieldData.maxValue+1)
                * assmtWaveToWaveDataMap[currentWave].assmtFieldMaxValModifier);
            totalTarget += fieldData.targetValue;
            fieldData.currentValue = 0;
            assignmentFields.Add(fieldData);

            availableFields.Remove(randomField);
        }

        assignment.fields = assignmentFields;
        assignment.timeRemaining = UnityEngine.Random.Range(minAssmtSecsPerTarget*totalTarget, (maxAssmtSecsPerTarget + 1)*totalTarget);

        //DisplayAssignmentData(assignment);
        assmtsSpawnedInCurrentWave++;
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
                audioSource.PlayOneShot(assmtMissedClip);
                RemoveAssmt(currentAssmt);
                gameStateController.ReduceLife();
                CheckForWaveEnd();
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

    private bool IsCurrentWaveCompleted()
    {
        return assmtQ.Count <= 0 && assmtsSpawnedInCurrentWave >= assmtWaveToWaveDataMap[currentWave].numAssmts;
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
        if (assmtQ.Count > 0 && activeAssignmentIndex >= 0)
        {
            return assmtQ[activeAssignmentIndex];
        }
        else
        {
            return new S_Assignment();
        }
    }

    public void SetActiveAssmt(S_Assignment assignment)
    {
        assmtQ[activeAssignmentIndex] = assignment;
        assmtSlots[activeAssignmentIndex].UpdateUI(assignment);
    }

    private void CheckForWaveEnd()
    {
        if (IsCurrentWaveCompleted())
        {
            if (currentWave >= assmtWaveToWaveDataMap.Count - 1)
            {
                audioSource.PlayOneShot(winClip);
                gameStateController.OnWin();
            }
            else
            {
                audioSource.PlayOneShot(waveEndClip);
                gameStateController.OnWaveCompleted(assmtWaveToWaveDataMap[currentWave].waveCompletionMsg);
                StartNewWave();
            }
        }
    }

    private int CalculateAssmtScore(S_Assignment assignment)
    {
        int score = 0;
        foreach(var field in assignment.fields)
        {
            score += field.currentValue;
        }
        return score;
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

    public List<S_Assignment> GetAssmtQ()
    {
        return assmtQ;
    }

    public void SubmitCompletedAssmts()
    {
        for (int i=assmtQ.Count-1; i>=0; i--)
        {
            if (IsAssmtCompleted(assmtQ[i]))
            {
                audioSource.PlayOneShot(assmtSubmittedClip);
                gameStateController.UpdateScore(CalculateAssmtScore(assmtQ[i]));
                RemoveAssmt(assmtQ[i]);
                CheckForWaveEnd();
            }
        }
    }
}
