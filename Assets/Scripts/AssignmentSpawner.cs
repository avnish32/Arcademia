using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AssignmentSpawner : MonoBehaviour
{
    [SerializeField]
    S_AssignmentFieldData[] assignmentFieldData;
    
    int minFields=2;

    private Dictionary<E_AssignmentFields, S_AssignmentFieldData> assignmentFieldToFieldDataMap;

    // Start is called before the first frame update
    void Start()
    {
        assignmentFieldToFieldDataMap = new Dictionary<E_AssignmentFields, S_AssignmentFieldData>();
        foreach (S_AssignmentFieldData fieldDatum in assignmentFieldData)
        {
            assignmentFieldToFieldDataMap[fieldDatum.field] = fieldDatum;
        }
        //Debug.Log("field data map count: " + assignmentFieldToFieldDataMap.Count);

        //InvokeRepeating("SpawnAssignment", 0f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<S_AssignmentFieldData> SpawnAssignment()
    {
        List<S_AssignmentFieldData> assignment = new List<S_AssignmentFieldData> ();
        List<E_AssignmentFields> availableFields = new List<E_AssignmentFields>(assignmentFieldToFieldDataMap.Keys);

        int numFields = UnityEngine.Random.Range(minFields, assignmentFieldToFieldDataMap.Count+1);

        for (int i = 0; i < numFields; i++)
        {
            E_AssignmentFields randomField = availableFields[UnityEngine.Random.Range (0, availableFields.Count)];
            
            S_AssignmentFieldData fieldData = assignmentFieldToFieldDataMap[randomField];
            fieldData.targetValue = UnityEngine.Random.Range(fieldData.minValue, fieldData.maxValue+1);
            fieldData.currentValue = 0;
            assignment.Add(fieldData);

            availableFields.Remove(randomField);
        }

        DisplayAssignmentData(assignment);
        return assignment;
    }

    private void DisplayAssignmentData(List<S_AssignmentFieldData> assignment)
    {
        string assignmentDataString = "";
        foreach (var field in assignment)
        {
            assignmentDataString += field.field + ": " + field.currentValue + "/" + field.targetValue + "\n";
        }
        
        Debug.Log(assignmentDataString);
    }
}
